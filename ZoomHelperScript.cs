using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Helper script containing functions that zooms the appropriate camera to the appropriate
 * neural pathways. Call using Zoom(animalName), and ZoomOut(animalName). Fades out the head 
 * mesh and dims the brain mesh of selected model when zoomed in. Alpha restored to default 
 * when zoomed out.
 */

public class ZoomHelperScript : MonoBehaviour {
    //drag and drop GameObjects containing appropriate mesh renderers
    [SerializeField] GameObject CoyoteMesh;
    [SerializeField] GameObject CoyoteBrainMesh;
    [SerializeField] GameObject HumanMesh;
    [SerializeField] GameObject HumanBrainMesh;
    [SerializeField] GameObject DolphinMesh;
    [SerializeField] GameObject DolphinBrainMesh;

    //drag and drop cameras
    [SerializeField] Animator CoyoteAnimator;
    [SerializeField] Animator HumanAnimator;
    [SerializeField] Animator DolphinAnimator;

    //MeshRenderer arrays to hold mesh renderers from above game objects
    static MeshRenderer[] CoyoteRenderer;
    static MeshRenderer[] CoyoteBrainRenderer;
    static MeshRenderer[] HumanRenderer;
    static MeshRenderer[] HumanBrainRenderer;
    static MeshRenderer[] DolphinRenderer;
    static MeshRenderer[] DolphinBrainRenderer;

    //Color objects to change alpha when zooming in or out
    static Color dim, fade, restore;

    //floats to hold r, g, b, and alpha values of mesh renderers
    float r, g, b, a;

    // Use this for initialization
    void Start () {
        //fill MeshRenderer[] arrays with renderers from appropriate GameObjects
        CoyoteRenderer = CoyoteMesh.GetComponentsInChildren<MeshRenderer>();
        CoyoteBrainRenderer = CoyoteBrainMesh.GetComponentsInChildren<MeshRenderer>();
        HumanRenderer = HumanMesh.GetComponentsInChildren<MeshRenderer>();
        HumanBrainRenderer = HumanBrainMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinRenderer = DolphinMesh.GetComponentsInChildren<MeshRenderer>();
        DolphinBrainRenderer = DolphinBrainMesh.GetComponentsInChildren<MeshRenderer>();

        //save default r, g, b, alpha values.
        //will require more variables if default values are not identical across meshes
        r = CoyoteRenderer[0].material.color.r;
        g = CoyoteRenderer[0].material.color.g;
        b = CoyoteRenderer[0].material.color.b;
        a = CoyoteRenderer[0].material.color.a;

        //set dim, fade, and restore colors.
        dim = new Color(r, g, b, 0.005f);
        fade = new Color(r, g, b, 0);
        restore = new Color(r, g, b, a);
    }

    //public zoom out functions
    public void Zoom(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            CoyoteAnimator.ResetTrigger("zoom out from coyote");
            CoyoteAnimator.SetTrigger("coyote");
        }
        else if (animalName.Equals("human"))
        {
            HumanAnimator.ResetTrigger("zoom out from human");
            HumanAnimator.SetTrigger("human");
        }
        else if (animalName.Equals("dolphin"))
        {
            DolphinAnimator.ResetTrigger("zoom out from dolphin");
            DolphinAnimator.SetTrigger("dolphin");
        }

        //fade out animal mesh and dim the brain mesh
        Fade(animalName);
        Dim(animalName);
    }

    //public zoom out functions
    public void ZoomOut(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            CoyoteAnimator.ResetTrigger("coyote");
            CoyoteAnimator.SetTrigger("zoom out from coyote");
        }
        else if (animalName.Equals("human"))
        {
            HumanAnimator.ResetTrigger("human");
            HumanAnimator.SetTrigger("zoom out from human");
        }
        else if (animalName.Equals("dolphin"))
        {
            DolphinAnimator.ResetTrigger("dolphin");
            DolphinAnimator.SetTrigger("zoom out from dolphin");
        }

        //restore alpha of animal head and brain to default
        Restore(animalName);
        RestoreBrain(animalName);
    }

    //functions that loop through the appropriate MeshRenderer[] arrays to change the alpha
    static void Fade(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            for (int i = 0; i < CoyoteRenderer.Length; i++)
            {
                CoyoteRenderer[i].material.color = fade;
            }
        }
        else if (animalName.Equals("human"))
        {
            for (int i = 0; i < HumanRenderer.Length; i++)
            {
                HumanRenderer[i].material.color = fade;
            }
        }
        else if (animalName.Equals("dolphin"))
        {
            for (int i = 0; i < DolphinRenderer.Length; i++)
            {
                DolphinRenderer[i].material.color = fade;
            }
        }
    }

    static void Dim(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            for (int i = 0; i < CoyoteBrainRenderer.Length; i++)
            {
                CoyoteBrainRenderer[i].material.color = dim;
            }
        }
        else if (animalName.Equals("human"))
        {
            for (int i = 0; i < HumanBrainRenderer.Length; i++)
            {
                HumanBrainRenderer[i].material.color = dim;
            }
        }
        else if (animalName.Equals("dolphin"))
        {
            for (int i = 0; i < DolphinBrainRenderer.Length; i++)
            {
                DolphinBrainRenderer[i].material.color = dim;
            }
        }
    }

    static void Restore(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            for (int i = 0; i < CoyoteRenderer.Length; i++)
            {
                CoyoteRenderer[i].material.color = restore;
            }
        }
        else if (animalName.Equals("human"))
        {
            for (int i = 0; i < HumanRenderer.Length; i++)
            {
                HumanRenderer[i].material.color = restore;
            }
        }
        else if (animalName.Equals("dolphin"))
        {
            for (int i = 0; i < DolphinRenderer.Length; i++)
            {
                DolphinRenderer[i].material.color = restore;
            }
        }
    }

    static void RestoreBrain(string animalName)
    {
        if (animalName.Equals("coyote"))
        {
            for (int i = 0; i < CoyoteBrainRenderer.Length; i++)
            {
                CoyoteBrainRenderer[i].material.color = restore;
            }
        }
        else if (animalName.Equals("human"))
        {
            for (int i = 0; i < HumanBrainRenderer.Length; i++)
            {
                HumanBrainRenderer[i].material.color = restore;
            }
        }
        else if (animalName.Equals("dolphin"))
        {
            for (int i = 0; i < DolphinBrainRenderer.Length; i++)
            {
                DolphinBrainRenderer[i].material.color = restore;
            }
        }
    }
}
