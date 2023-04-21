using UnityEngine;
using UnityEditor;
using System.IO;

namespace MTAssets.MobileInputControls.Editor
{

    /*
     * This class is responsible for creating the menu for this asset. 
     */

    public class Menu : MonoBehaviour
    {
        //Right click menu items

        [MenuItem("GameObject/Mobile Controls/Joystick Axis", false, 30)]
        static void CreateJoystickAxis(MenuCommand command)
        {
            //Get the gameobject of right click
            GameObject gameObjectOfClick = (GameObject)command.context;

            //If has not clicked in a component of canvas
            if (gameObjectOfClick == null || gameObjectOfClick.GetComponent<RectTransform>() == null)
            {
                EditorUtility.DisplayDialog("Error", "Select a GameObject that is inside a Canvas or a Canvas to create a Joystick Axis.", "Ok");
                return;
            }

            //If has clicked in a component of canvas
            if (gameObjectOfClick.GetComponent<RectTransform>() != null)
            {
                GameObject joystickAxis = new GameObject("Joystick Axis");
                RectTransform rect = joystickAxis.AddComponent<RectTransform>();
                joystickAxis.AddComponent<JoystickAxis>();
                if (gameObjectOfClick != null)
                {
                    joystickAxis.transform.SetParent(gameObjectOfClick.transform);
                    rect.localPosition = Vector3.zero;
                    rect.localRotation = Quaternion.Euler(0, 0, 0);
                }
                Selection.objects = new GameObject[] { joystickAxis };
            }
        }

        [MenuItem("GameObject/Mobile Controls/Drag Area", false, 30)]
        static void CreateDragArea(MenuCommand command)
        {
            //Get the gameobject of right click
            GameObject gameObjectOfClick = (GameObject)command.context;

            //If has not clicked in a component of canvas
            if (gameObjectOfClick == null || gameObjectOfClick.GetComponent<RectTransform>() == null)
            {
                EditorUtility.DisplayDialog("Error", "Select a GameObject that is inside a Canvas or a Canvas to create a Drag Area.", "Ok");
                return;
            }

            //If has clicked in a component of canvas
            if (gameObjectOfClick.GetComponent<RectTransform>() != null)
            {
                GameObject dragArea = new GameObject("Drag Area");
                RectTransform rect = dragArea.AddComponent<RectTransform>();
                dragArea.AddComponent<DragArea>();
                if (gameObjectOfClick != null)
                {
                    dragArea.transform.SetParent(gameObjectOfClick.transform);
                    rect.localPosition = Vector3.zero;
                    rect.localRotation = Quaternion.Euler(0, 0, 0);
                }
                Selection.objects = new GameObject[] { dragArea };
            }
        }

        //Menu items

        [MenuItem("Tools/MT Assets/Mobile Input Controls/Changelog", false, 10)]
        static void OpenChangeLog()
        {
            string filePath = "Assets/MT Assets/Mobile Input Controls/List Of Changes.txt";

            if (File.Exists(filePath) == true)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset)));
            }
            if (File.Exists(filePath) == false)
            {
                EditorUtility.DisplayDialog("Error", "Unable to open file. The file has been deleted, or moved. Please, to correct this problem and avoid future problems with this tool, remove all files from this asset and install it again.", "Ok");
            }
        }

        [MenuItem("Tools/MT Assets/Mobile Input Controls/Read Documentation", false, 30)]
        static void ReadDocumentation()
        {
            EditorUtility.DisplayDialog("Read Documentation", "The documentation is located inside the \n\"MT Assets/Mobile Input Controls\" folder. Just unzip \"Documentation.zip\" on your desktop and open the \"Documentation.html\" file with your preferred browser.", "Cool!");
        }

        [MenuItem("Tools/MT Assets/Mobile Input Controls/More Assets", false, 30)]
        static void MoreAssets()
        {
            Application.OpenURL("https://assetstore.unity.com/publishers/40306");
        }

        [MenuItem("Tools/MT Assets/Mobile Input Controls/Support", false, 30)]
        static void GetSupport()
        {
            EditorUtility.DisplayDialog("Support", "If you have any questions, problems or want to contact me, just contact me by email (mtassets@windsoft.xyz).", "Got it!");
        }
    }
}