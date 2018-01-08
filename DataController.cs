/* Loads in text from the .json file dynamically.
 * Provides methods to set colors, alpha, data, etc based on input.
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    //variables to store text data from the .json file
    private AnimalData[] allAnimalData;
    private AnimalData coyoteData, humanData, dolphinData;

    private string animalDataFileName = "data.json";

    //Camera canvas determines the name of the activated sense,
    //the worldspace canvas determines the information about the selected sense
    public Canvas coyoteCameraCanvas, coyoteWorldSpaceCanvas, coyoteInstructionsCanvas;
    private Text[] coyoteTitle, coyoteActivation, coyoteInstructions;

    public Canvas humanCameraCanvas, humanWorldSpaceCanvas, humanInstructionsCanvas;
    private Text[] humanTitle, humanActivation, humanInstructions;

    public Canvas dolphinCameraCanvas, dolphinWorldSpaceCanvas, dolphinInstructionsCanvas;
    private Text[] dolphinTitle, dolphinActivation, dolphinInstructions;

    private Image[] coyoteImages, humanImages, dolphinImages;

    private Color orange, yellow, blue, magenta;

    //Strings for instructions
    private string blank, touch, turn;

    //alpha value for fadeout of information not in front
    public float fadeAlpha = 0.10f;

    //value for facts spacing
    public float textOneSpacing, textTwoSpacing = 0;
    private Vector3 activationDataOnePos, activationDataTwoPos;
    private Quaternion activationDataOneRot, activationDataTwoRot;

    //models needed for rotation values in order to determine
    //which text boxes to fade out
    public GameObject coyote, human, dolphin;


    // Use this for initialization

    void Start()
    {
        //load game data from .json file
        LoadGameData();

        //load individual animal data into appropriate objects
        coyoteData = allAnimalData[0];
        humanData = allAnimalData[1];
        dolphinData = allAnimalData[2];

        //get references to the appropriate text boxes on the canvas
        coyoteTitle = coyoteCameraCanvas.GetComponentsInChildren<Text>();
        coyoteActivation = coyoteWorldSpaceCanvas.GetComponentsInChildren<Text>();

        humanTitle = humanCameraCanvas.GetComponentsInChildren<Text>();
        humanActivation = humanWorldSpaceCanvas.GetComponentsInChildren<Text>();

        dolphinTitle = dolphinCameraCanvas.GetComponentsInChildren<Text>();
        dolphinActivation = dolphinWorldSpaceCanvas.GetComponentsInChildren<Text>();

        //get references to the appropriate image boxes on the canvas
        coyoteInstructions = coyoteInstructionsCanvas.GetComponentsInChildren<Text>();
        coyoteImages = coyoteInstructionsCanvas.GetComponentsInChildren<Image>();
        humanInstructions = humanInstructionsCanvas.GetComponentsInChildren<Text>();
        humanImages = humanInstructionsCanvas.GetComponentsInChildren<Image>();
        dolphinInstructions = dolphinInstructionsCanvas.GetComponentsInChildren<Text>();
        dolphinImages = dolphinInstructionsCanvas.GetComponentsInChildren<Image>();


        //set the name text w/ the name of the animals
        coyoteTitle[0].text = coyoteData.name;
        humanTitle[0].text = humanData.name;
        dolphinTitle[0].text = dolphinData.name;

        //unload the information data from the UI
        UnloadRuntimeData("coyote");
        UnloadRuntimeData("human");
        UnloadRuntimeData("dolphin");
        UnloadInstructions("coyote");
        UnloadInstructions("human");
        UnloadInstructions("dolphin");

        //set string values for instructions
        touch = "TOUCH a Sense";
        turn = "TURN the Head";


        //orange = coyoteTitle[0].color;
        //get the color values for yellow/magenta/blue
        yellow = dolphinTitle[1].color;
        magenta = coyoteTitle[1].color;
        blue = humanTitle[1].color;
    }

    /* Method to load appropriate information to the worldspace UI
     * based on animal and the selected sense
     */
    public void LoadRuntimeData(string animal, string sense)
    {
        //declare variables to be used
        SensesData senseData = null;
        AnimalData animalData = null;
        Text[] activationData = new Text[6];
        Text[] titleData = new Text[2];
        Color senseColor = new Color();
        GameObject model = null;

        //define variables to be used based on animal parameter
        if (animal.Equals("coyote"))
        {
            animalData = coyoteData;
            titleData = coyoteTitle;
            activationData = coyoteActivation;
            model = coyote;
        }
        else if (animal.Equals("human"))
        {
            animalData = humanData;
            titleData = humanTitle;
            activationData = humanActivation;
            model = human;
        }
        else if (animal.Equals("dolphin"))
        {
            animalData = dolphinData;
            titleData = dolphinTitle;
            activationData = dolphinActivation;
            model = dolphin;
            activationData[4].text = "";
            activationData[5].text = "";
        }

        //define sense name with assigned color based on sense
        if (sense.Equals("visual"))
        {
            senseData = animalData.senses[0];
            senseColor = magenta;
        }
        else if (sense.Equals("auditory"))
        {
            senseData = animalData.senses[1];
            senseColor = yellow;
        }
        else if (sense.Equals("olfactory"))
        {
            senseData = animalData.senses[2];
            senseColor = blue;
        }

        //display sense name and sense information
        titleData[1].text = senseData.name;
        titleData[1].color = senseColor;
        activationData[0].text = senseData.activation;
        activationData[0].color = senseColor;
        activationData[1].text = senseData.facts[0];
        activationData[2].text = senseData.facts[1];
        activationData[3].text = senseData.facts[2];

        if (animal.Equals("dolphin"))
        {
            if (senseData.facts[2].Equals(""))
            {
                activationData[4].text = senseData.facts[0];
                activationData[5].text = senseData.facts[1];
                activationData[1].text = "";
                activationData[2].text = "";
                activationData[3].text = "";
            }
        }

        //fade out text data if they aren't in front
        FadeText(activationData, senseColor, model);
    }

    /* Method to make the sense name and data text boxes to be blank
     */
    public void UnloadRuntimeData(string animal)
    {
        //declare variables to be used
        Text[] activationData = new Text[6];
        Text[] titleData = new Text[2];

        //define variables based on animal
        if (animal.Equals("coyote"))
        {
            activationData = coyoteActivation;
            titleData = coyoteTitle;
        }
        else if (animal.Equals("human"))
        {
            activationData = humanActivation;
            titleData = humanTitle;
        }
        else if (animal.Equals("dolphin"))
        {
            activationData = dolphinActivation;
            titleData = dolphinTitle;
            activationData[4].text = "";
            activationData[5].text = "";
        }

        //set sense name and information texts to be blank
        titleData[1].text = "";
        activationData[0].text = "";
        activationData[1].text = "";
        activationData[2].text = "";
        activationData[3].text = "";
    }

    /* Method to make the instructions to appear
     */
    public void LoadInstructions(string animal, string instruction)
    {
        Text[] instructions = null;
        Image[] images = null;

        if (animal.Equals("coyote"))
        {
            instructions = coyoteInstructions;
            images = coyoteImages;
        }
        else if (animal.Equals("human"))
        {
            instructions = humanInstructions;
            images = humanImages;
        }
        else if (animal.Equals("dolphin"))
        {
            instructions = dolphinInstructions;
            images = dolphinImages;
        }

        if (instruction.Equals("touch"))
        {
            for (int i = 0; i < instructions.Length / 2; i++)
                instructions[i].CrossFadeAlpha(1, 1, true);
            images[0].CrossFadeAlpha(1, 1, true);
        }
        else
        {
            for (int i = instructions.Length / 2; i < instructions.Length; i++)
                instructions[i].CrossFadeAlpha(1, 1, true);
            images[1].CrossFadeAlpha(1, 1, true);
        }
    }

    /* Method to make the instructions to be blank
     */
    public void UnloadInstructions(string animal)
    {
        Text[] instructions = null;
        Image[] images = null;

        if (animal.Equals("coyote"))
        {
            instructions = coyoteInstructions;
            images = coyoteImages;
        }
        else if (animal.Equals("human"))
        {
            instructions = humanInstructions;
            images = humanImages;
        }
        else if (animal.Equals("dolphin"))
        {
            instructions = dolphinInstructions;
            images = dolphinImages;
        }

        for (int i = 0; i < instructions.Length; i++)
            instructions[i].CrossFadeAlpha(0, 0, true);
        for (int i = 0; i < images.Length; i++)
            images[i].CrossFadeAlpha(0, 0, true);
    }

    /* Method to retrieve data from the .json file
     */
    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, animalDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            InteractiveData loadedData = JsonUtility.FromJson<InteractiveData>(dataAsJson);

            // Retrieve the allAnimalData property of loadedData
            allAnimalData = loadedData.allAnimalData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    /* Method to fade texts that are not in front (facing the display)
     */
    private void FadeText(Text[] texts, Color senseColor, GameObject model)
    {
        //set the faded colors for the main activation text and fact texts
        Color whiteFaded = new Color(1, 1, 1, fadeAlpha);
        Color colorFaded = new Color(senseColor.r, senseColor.g, senseColor.b, fadeAlpha);

        //set colors of the text boxes according to the model's rotation
        if (!texts[1].text.Equals(""))
        {
            if (model.transform.eulerAngles.y < 45 || model.transform.eulerAngles.y > 315)
            {
                texts[0].color = senseColor;
                texts[1].color = texts[2].color = texts[3].color = whiteFaded;
            }
            else if (model.transform.eulerAngles.y > 45 && model.transform.eulerAngles.y < 135)
            {
                texts[1].color = Color.white;
                texts[0].color = colorFaded;
                texts[2].color = texts[3].color = whiteFaded;
            }
            else if (model.transform.eulerAngles.y > 135 && model.transform.eulerAngles.y < 225)
            {
                texts[2].color = Color.white;
                texts[0].color = colorFaded;
                texts[1].color = texts[3].color = whiteFaded;
            }
            else if (model.transform.eulerAngles.y > 225 && model.transform.eulerAngles.y < 315)
            {
                texts[3].color = Color.white;
                texts[0].color = colorFaded;
                texts[1].color = texts[2].color = whiteFaded;
            }
        }
        else
        {
            if (model.transform.eulerAngles.y < 60 || model.transform.eulerAngles.y > 300)
            {
                texts[0].color = senseColor;
                texts[4].color = texts[5].color = whiteFaded;
            }
            else if (model.transform.eulerAngles.y > 60 && model.transform.eulerAngles.y < 180)
            {
                texts[4].color = Color.white;
                texts[0].color = colorFaded;
                texts[5].color = whiteFaded;
            }
            else if (model.transform.eulerAngles.y > 180 && model.transform.eulerAngles.y < 300)
            {
                texts[5].color = Color.white;
                texts[0].color = colorFaded;
                texts[4].color = whiteFaded;
            }
        }

    }
}