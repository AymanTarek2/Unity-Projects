#include <stdio.h>
#include <stdint.h>
#include <stdbool.h>
#include <unistd.h>
#include "freertos/FreeRTOS.h"
#include "freertos/task.h"
#include "driver/gpio.h"
//#include "driver/mcpwm.h"
#include "soc/mcpwm_periph.h"
#include "driver/i2c.h"
#include "esp_err.h"
#include "esp_log.h"
#include "driver/mcpwm_prelude.h"
#include "driver/uart.h"

// Definitions for UART and GPIO
#define UART_NUM UART_NUM_1
#define GPIO_OUTPUT_PIN 13
#define BUF_SIZE (1024)

//servo
#define SERVO_MIN_PULSEWIDTH_US 500  // Minimum pulse width in microsecond
#define SERVO_MAX_PULSEWIDTH_US 2500  // Maximum pulse width in microsecond
#define SERVO_MIN_DEGREE        -90   // Minimum angle
#define SERVO_MAX_DEGREE        90    // Maximum angle

#define SERVO_PULSE_GPIO             4        // GPIO connects to the PWM signal line
#define SERVO_TIMEBASE_RESOLUTION_HZ 1000000  // 1MHz, 1us per tick
#define SERVO_TIMEBASE_PERIOD        20000    // 20000 ticks, 20ms

mcpwm_cmpr_handle_t comparator = NULL;

void servo_init(){
    mcpwm_timer_handle_t timer = NULL;
    mcpwm_timer_config_t timer_config = {
        .group_id = 0,
        .clk_src = MCPWM_TIMER_CLK_SRC_DEFAULT,
        .resolution_hz = SERVO_TIMEBASE_RESOLUTION_HZ,
        .period_ticks = SERVO_TIMEBASE_PERIOD,
        .count_mode = MCPWM_TIMER_COUNT_MODE_UP,
    };
    ESP_ERROR_CHECK(mcpwm_new_timer(&timer_config, &timer));

    mcpwm_oper_handle_t oper = NULL;
    mcpwm_operator_config_t operator_config = {
        .group_id = 0, // operator must be in the same group to the timer
    };
    ESP_ERROR_CHECK(mcpwm_new_operator(&operator_config, &oper));

    ESP_ERROR_CHECK(mcpwm_operator_connect_timer(oper, timer));

    
    mcpwm_comparator_config_t comparator_config = {
        .flags.update_cmp_on_tez = true,
    };
    ESP_ERROR_CHECK(mcpwm_new_comparator(oper, &comparator_config, &comparator));

    mcpwm_gen_handle_t generator = NULL;
    mcpwm_generator_config_t generator_config = {
        .gen_gpio_num = SERVO_PULSE_GPIO,
    };
    ESP_ERROR_CHECK(mcpwm_new_generator(oper, &generator_config, &generator));

    // set the initial compare value, so that the servo will spin to the center position
    ESP_ERROR_CHECK(mcpwm_comparator_set_compare_value(comparator, example_angle_to_compare(0)));

    // go high on counter empty
    ESP_ERROR_CHECK(mcpwm_generator_set_action_on_timer_event(generator,
                                                              MCPWM_GEN_TIMER_EVENT_ACTION(MCPWM_TIMER_DIRECTION_UP, MCPWM_TIMER_EVENT_EMPTY, MCPWM_GEN_ACTION_HIGH)));
    // go low on compare threshold
    ESP_ERROR_CHECK(mcpwm_generator_set_action_on_compare_event(generator,
                                                                MCPWM_GEN_COMPARE_EVENT_ACTION(MCPWM_TIMER_DIRECTION_UP, comparator, MCPWM_GEN_ACTION_LOW)));

    ESP_ERROR_CHECK(mcpwm_timer_enable(timer));
    ESP_ERROR_CHECK(mcpwm_timer_start_stop(timer, MCPWM_TIMER_START_NO_STOP));

}

void uart_init(){
	// Configure UART parameters
    const uart_config_t uart_config = {
        .baud_rate = 115200,
        .data_bits = UART_DATA_8_BITS,
        .parity = UART_PARITY_DISABLE,
        .stop_bits = UART_STOP_BITS_1,
        .flow_ctrl = UART_HW_FLOWCTRL_DISABLE,
    };
    uart_param_config(UART_NUM, &uart_config);
    uart_set_pin(UART_NUM, GPIO_NUM_1, GPIO_NUM_3, UART_PIN_NO_CHANGE, UART_PIN_NO_CHANGE); // TX, RX
    uart_driver_install(UART_NUM, BUF_SIZE * 2, 0, 0, NULL, 0);

    // Configure GPIO
     esp_rom_gpio_pad_select_gpio(GPIO_OUTPUT_PIN);
    gpio_set_direction(GPIO_OUTPUT_PIN, GPIO_MODE_OUTPUT);
}


void servo_set_angle(int angle){
	uint32_t conversion = (angle - SERVO_MIN_DEGREE) 
	* (SERVO_MAX_PULSEWIDTH_US - SERVO_MIN_PULSEWIDTH_US) / (SERVO_MAX_DEGREE - SERVO_MIN_DEGREE) + SERVO_MIN_PULSEWIDTH_US;
	
	ESP_ERROR_CHECK(mcpwm_comparator_set_compare_value(comparator, conversion));
}

void app_main(void) {
    servo_init();
    uart_init();
    
    char rx_buffer[BUF_SIZE];
    int rx_index = 0;
    int data = 0;
    
    while (1) {
        uint8_t received_char;
        const int rxBytes = uart_read_bytes(UART_NUM, &received_char, 1, 20 / portTICK_PERIOD_MS);
        
        if (rxBytes > 0) {
            if ((received_char >= '0' && received_char <= '9') || received_char == '-') {
                rx_buffer[rx_index++] = received_char;
            }
            
            if (received_char == ',') {
                rx_buffer[rx_index] = '\0';
                data = atoi(rx_buffer);
                rx_index = 0;
                servo_set_angle(data * -1);
            }
        } 
    }
}
