using UnityEngine;

public class ArticulationSetup : MonoBehaviour
{
    public GameObject centralObject;
    public GameObject[] arm1Links = new GameObject[3];
    public GameObject[] arm2Links = new GameObject[3];
    public GameObject[] arm3Links = new GameObject[3];
    public GameObject[] arm4Links = new GameObject[3];

    private ArticulationBody[][] allArms;

    // Adjustable rotation speed multiplier
    public float rotationSpeed = 0.0001f; // Adjust as needed

    void Start()
    {
        SetupArm(centralObject, arm1Links);
        SetupArm(centralObject, arm2Links);
        SetupArm(centralObject, arm3Links);
        SetupArm(centralObject, arm4Links);

        // Store references to all articulation bodies for easy access
        allArms = new ArticulationBody[][]
        {
            GetArticulationBodies(arm1Links),
            GetArticulationBodies(arm2Links),
            GetArticulationBodies(arm3Links),
            GetArticulationBodies(arm4Links)
        };
    }

    void SetupArm(GameObject parent, GameObject[] links)
    {
        GameObject previous = parent;

        for (int i = 0; i < links.Length; i++)
        {
            // Check if the ArticulationBody component already exists
            ArticulationBody ab = links[i].GetComponent<ArticulationBody>();
            if (ab == null)
            {
                // Add ArticulationBody component if it doesn't exist
                ab = links[i].AddComponent<ArticulationBody>();
            }

            ab.jointType = ArticulationJointType.RevoluteJoint;

            // Set appropriate mass and inertia
            ab.mass = 2.2f;
            ab.linearDamping = 0.01f;
            ab.angularDamping = 0.01f;

            // Set anchor position to the local position of the link relative to its parent
            ab.anchorPosition = previous.transform.InverseTransformPoint(links[i].transform.position);
            ab.parentAnchorPosition = ab.anchorPosition;

            // Define the rotation axis based on the link index
            if (i == 0)
            {
                // Rotate around the y-axis for link1
                ab.anchorRotation = Quaternion.Euler(0, 0, 0);
                ab.swingYLock = ArticulationDofLock.LimitedMotion;
                ab.swingZLock = ArticulationDofLock.LockedMotion;
                ab.twistLock = ArticulationDofLock.LockedMotion;
            }
            else
            {
                // Rotate around the x and z axes for link2 and link3
                ab.anchorRotation = Quaternion.Euler(0, 0, 0);
                ab.swingYLock = ArticulationDofLock.LockedMotion;
                ab.swingZLock = ArticulationDofLock.LimitedMotion;
                ab.twistLock = ArticulationDofLock.LimitedMotion;
            }

            // Set joint limits
            var jointDrive = ab.xDrive;
            jointDrive.lowerLimit = -45.0f; // Set appropriate limits
            jointDrive.upperLimit = 45.0f;
            jointDrive.stiffness = 10;
            jointDrive.damping = 10;
            jointDrive.forceLimit = 10;
            ab.xDrive = jointDrive;

            // Set parent-child relationship
            links[i].transform.SetParent(previous.transform);
            previous = links[i];
        }
    }

    ArticulationBody[] GetArticulationBodies(GameObject[] links)
    {
        ArticulationBody[] bodies = new ArticulationBody[links.Length];
        for (int i = 0; i < links.Length; i++)
        {
            bodies[i] = links[i].GetComponent<ArticulationBody>();
        }
        return bodies;
    }

    void Update()
    {
        // Example key bindings for rotating each link of all arms
        // Modify key bindings as needed for your control scheme

        // Arm 1
        if (Input.GetKey(KeyCode.Q))
        {
            RotateLink(allArms[0][0], 1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            RotateLink(allArms[0][0], -1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.W))
        {
            RotateLink(allArms[0][1], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.S))
        {
            RotateLink(allArms[0][1], -1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.E))
        {
            RotateLink(allArms[0][2], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotateLink(allArms[0][2], -1, ArticulationAxis.X);
        }

        // Arm 2
        if (Input.GetKey(KeyCode.R))
        {
            RotateLink(allArms[1][0], 1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.F))
        {
            RotateLink(allArms[1][0], -1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.T))
        {
            RotateLink(allArms[1][1], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.G))
        {
            RotateLink(allArms[1][1], -1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            RotateLink(allArms[1][2], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.H))
        {
            RotateLink(allArms[1][2], -1, ArticulationAxis.X);
        }

        // Arm 3
        if (Input.GetKey(KeyCode.U))
        {
            RotateLink(allArms[2][0], 1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.J))
        {
            RotateLink(allArms[2][0], -1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.I))
        {
            RotateLink(allArms[2][1], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.K))
        {
            RotateLink(allArms[2][1], -1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.O))
        {
            RotateLink(allArms[2][2], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.L))
        {
            RotateLink(allArms[2][2], -1, ArticulationAxis.X);
        }

        // Arm 4
        if (Input.GetKey(KeyCode.P))
        {
            RotateLink(allArms[3][0], 1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.Semicolon))
        {
            RotateLink(allArms[3][0], -1, ArticulationAxis.Y);
        }
        if (Input.GetKey(KeyCode.LeftBracket))
        {
            RotateLink(allArms[3][1], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.Quote))
        {
            RotateLink(allArms[3][1], -1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.RightBracket))
        {
            RotateLink(allArms[3][2], 1, ArticulationAxis.X);
        }
        if (Input.GetKey(KeyCode.Backslash))
        {
            RotateLink(allArms[3][2], -1, ArticulationAxis.X);
        }
    }

    void RotateLink(ArticulationBody ab, float direction, ArticulationAxis axis)
    {
        var drive = ab.xDrive;

        switch (axis)
        {
            case ArticulationAxis.X:
                drive = ab.xDrive;
                drive.target += direction * rotationSpeed;
                ab.xDrive = drive;
                break;
            case ArticulationAxis.Y:
                drive = ab.yDrive;
                drive.target += direction * rotationSpeed;
                ab.yDrive = drive;
                break;
            case ArticulationAxis.Z:
                drive = ab.zDrive;
                drive.target += direction * rotationSpeed;
                ab.zDrive = drive;
                break;
        }
    }

    enum ArticulationAxis
    {
        X,
        Y,
        Z
    }
}
