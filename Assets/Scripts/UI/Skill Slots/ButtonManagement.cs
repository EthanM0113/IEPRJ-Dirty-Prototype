using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManagement : MonoBehaviour
{
	private GameObject[] btn;

	[SerializeField] Color tint1;
	[SerializeField] Color tint2;

	int selected_Button = 0;

	float horizontal;
	float vertical;

	bool buttonpressed = false;

	GameObject firstButton;
    Vector3 lastMouseCoordinate = Vector3.zero;
	[SerializeField] bool isMouseMoving = false;
    [SerializeField] float mouseDuration = 1.0f;

    // Update is called once per frame
    void Update()
	{
        // First we find out how much it has moved, by comparing it with the stored coordinate.
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;

        // Then we check if it has moved to the left.
        if (mouseDelta != Vector3.zero) // Assuming a negative value is to the left.
            isMouseMoving = true;

        bool checkMouse = false;

        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButton(i))
            {
                checkMouse = true; break;
            }
        }

        if (!Input.GetKeyUp(KeyCode.Escape) && !checkMouse && Input.anyKeyDown)
        {
            isMouseMoving = false;
        }

        HandleKeyboardInput();


        // Then we store our mousePosition so that we can check it again next frame.
        lastMouseCoordinate = Input.mousePosition;
    }

	void HandleKeyboardInput()
	{
        if (isMouseMoving)
        {
            if (btn.Length > 0)
            {
                //Check if Panel changed, wenn ja aktiviere den ersten Button
                if (firstButton != btn[0])
                {
                    selected_Button = 0;
                }
                firstButton = btn[0];

                //Higlight the active Button
                for (int a = 0; a < btn.Length; a++)
                {
                    btn[a].GetComponent<Image>().sprite = btn[a].GetComponent<UISpecialBTN>().GetNormal();
                }
            }
            return;
        }
        // Make an array of all the buttons with the tag 'button'
        btn = GameObject.FindGameObjectsWithTag("Button");

        if (btn.Length > 0)
        {
            //Check if Panel changed, wenn ja aktiviere den ersten Button
            if (firstButton != btn[0])
            {
                selected_Button = 0;
            }
            firstButton = btn[0];

            //Higlight the active Button
            for (int a = 0; a < btn.Length; a++)
            {
                if (a == selected_Button)
                {
                    btn[a].GetComponent<Button>().Select();
                    btn[a].GetComponent<Image>().color = tint2;
                }
                else
                {
                    btn[a].GetComponent<Image>().color = tint1;
                }
            }

            //Get Position of all Buttons
            Vector3 but_pos = btn[selected_Button].GetComponent<RectTransform>().position;

            //Array for differences in the positions of buttons
            float[] hor_dif = new float[btn.Length];
            float[] vert_dif = new float[btn.Length];

            //Compute the differences in position of buttons
            for (int a = 0; a < btn.Length; a++)
            {
                if (a != selected_Button)
                {
                    Vector3 but_pos2 = btn[a].GetComponent<RectTransform>().position;
                    hor_dif[a] = but_pos.x - but_pos2.x;
                    vert_dif[a] = but_pos.y - but_pos2.y;
                }
            }

            //if a closer difference in positions for buttons is true, this will be overwriten
            float new_vert = 9999;
            float new_hor = 9999;

            if (Input.GetKeyDown(KeyCode.S) && !buttonpressed)
            {

                buttonpressed = true;

                for (int a = 0; a < btn.Length; a++)
                {
                    //Dont test for the selected buttons
                    if (a != selected_Button)
                    {
                        //Check for correct direction
                        if (vert_dif[a] > 0)
                        {
                            //Find closest Button in both direction
                            if (Mathf.Abs(hor_dif[a]) < Mathf.Abs(new_hor))
                            {
                                new_hor = hor_dif[a];
                                if (Mathf.Abs(vert_dif[a]) < new_vert)
                                {
                                    new_vert = vert_dif[a];
                                    selected_Button = a;
                                }
                                else
                                {
                                    selected_Button = a;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.W) && !buttonpressed)
            {

                buttonpressed = true;

                for (int a = 0; a < btn.Length; a++)
                {
                    if (a != selected_Button)
                    {
                        if (vert_dif[a] < 0)
                        {
                            if (Mathf.Abs(hor_dif[a]) <= Mathf.Abs(new_hor))
                            {
                                new_hor = hor_dif[a];
                                if (Mathf.Abs(vert_dif[a]) <= new_vert)
                                {
                                    new_vert = vert_dif[a];
                                    selected_Button = a;
                                }
                                else
                                {
                                    selected_Button = a;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && !buttonpressed)
            {

                buttonpressed = true;

                for (int a = 0; a < btn.Length; a++)
                {
                    if (a != selected_Button)
                    {
                        if (hor_dif[a] > 0)
                        {
                            if (Mathf.Abs(vert_dif[a]) < Mathf.Abs(new_vert))
                            {
                                new_vert = vert_dif[a];
                                if (Mathf.Abs(hor_dif[a]) < new_hor)
                                {
                                    new_hor = hor_dif[a];
                                    selected_Button = a;
                                }
                                else
                                {
                                    selected_Button = a;
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.D) && !buttonpressed)
            {

                buttonpressed = true;

                for (int a = 0; a < btn.Length; a++)
                {
                    if (a != selected_Button)
                    {
                        if (hor_dif[a] < 0)
                        {
                            if (Mathf.Abs(vert_dif[a]) < Mathf.Abs(new_vert))
                            {
                                new_vert = vert_dif[a];
                                if (Mathf.Abs(hor_dif[a]) < new_hor)
                                {
                                    new_hor = hor_dif[a];
                                    selected_Button = a;
                                }
                                else
                                {
                                    selected_Button = a;
                                }
                            }
                        }
                    }
                }
            }

            if (horizontal == 0 && vertical == 0) { buttonpressed = false; }


        }
        else { selected_Button = 0; }
    }
}
