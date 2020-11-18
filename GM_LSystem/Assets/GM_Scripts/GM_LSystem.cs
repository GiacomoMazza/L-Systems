using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_LSystem : MonoBehaviour
{
    //Members

    //The List of objects that make the L-System.
    private List<GameObject> l_AllMetres;

    //The number of iterations.
    public int in_Iterations;

    //Data stacks used to store the spawner's position and rotation.
    private Stack<Vector3> s_v3Pos;
    private Stack<Quaternion> s_qRot;

    //3D Object 1 metre long.
    public GameObject go_1Metre;

    //The characters that will store the user's input for the characters they want to use. generally they are F, and if need be, X. 
    public char ch_Axiom1;
    public char ch_Axiom2;
    //This store the original axiom. Used in the replacement process when there if more than 1 character (Ex: F and X are used as character 1 and character 2 instead of using only 1 character, F).
    private string st_Axiom;

    //References for character 1 and 2, respectively. Both used in the replacing process. Generally, they would be F and X.
    public char AOld;
    public char BOld;

    public char ANew; //A (To be written in the Inspector). Used in the replacing process.

    public char ch_F; //F (To be written in the Inspector). Used to assign the value of a character.
    public char ch_X; //X (To be written in the Inspector). Used to assign the value of a character.

    //Angle to rotate the spawner by.
    public float fl_Angle;

    //Rules for replacement. Rule 1 is the final rule (which is used in the replacing process), rule 2 is for character 1.
    public string st_Rule1;
    public string st_Rule2;
    //This stores the entire string from the L-System.
    public string st_Formula; 

    private string st_Original1; // Reference for Character 1, but in string format.
    private string st_Original2; // Reference for Character 2, but in string format.

    //InputFields to assign in the Inspector. Each one refers to one of the members above.
    public InputField if_Iterations;
    public InputField if_Character1;
    public InputField if_Character2;
    public InputField if_Rule1;
    public InputField if_Rule2;
    public InputField if_Angle;
    public InputField if_StartAxiom;

    /// <summary>
    /// Calles the Restart method, changes the data and pastes them in the Input Fields, and then calles the OnGenerator method.
    /// </summary>
    public void Example1()
    {
        Restart();
        in_Iterations = 7;
        if_Iterations.text = in_Iterations.ToString();
        fl_Angle = 25.7f;
        if_Angle.text = fl_Angle.ToString();
        ch_Axiom1 = ch_F;
        if_Character1.text = ch_Axiom1.ToString();
        ch_Axiom2 = ch_X;
        if_Character2.text = ch_Axiom2.ToString();
        st_Rule1 = "F[+X][-X]FX";
        if_Rule1.text = st_Rule1;
        st_Rule2 = "FF";
        if_Rule2.text = st_Rule2;
        st_Axiom = "F";
        if_StartAxiom.text = st_Axiom;
        OnGenerator();
    }

    /// <summary>
    /// Calles the Restart method, changes the data and pastes them in the Input Fields, and then calles the OnGenerator method.
    /// </summary>
    public void Example2()
    {
        Restart();
        in_Iterations = 5;
        if_Iterations.text = in_Iterations.ToString();
        fl_Angle = 25.7f;
        if_Angle.text = fl_Angle.ToString();
        ch_Axiom1 = ch_F;
        if_Character1.text = ch_Axiom1.ToString();
        ch_Axiom2 = char.MinValue;
        if_Character2.text = ch_Axiom2.ToString();
        st_Rule1 = "F[+F]F[-F]F";
        if_Rule1.text = st_Rule1;
        st_Rule2 = "";
        if_Rule2.text = st_Rule2;
        st_Axiom = "F";
        if_StartAxiom.text = st_Axiom;
        OnGenerator();
    }

    /// <summary>
    /// Calles the Restart method, changes the data and pastes them in the Input Fields, and then calles the OnGenerator method.
    /// </summary>
    public void Example3()
    {
        Restart();
        in_Iterations = 7;
        if_Iterations.text = in_Iterations.ToString();
        fl_Angle = 20f;
        if_Angle.text = fl_Angle.ToString();
        ch_Axiom1 = ch_F;
        if_Character1.text = ch_Axiom1.ToString();
        ch_Axiom2 = ch_X;
        if_Character2.text = ch_Axiom2.ToString();
        st_Rule1 = "F[+X]F[-X]+X";
        if_Rule1.text = st_Rule1;
        st_Rule2 = "FF";
        if_Rule2.text = st_Rule2;
        st_Axiom = "F";
        if_StartAxiom.text = st_Axiom;
        OnGenerator();
    }

    /// <summary>
    /// Calles the Restart method, changes the data and pastes them in the Input Fields, and then calles the OnGenerator method.
    /// </summary>
    public void Example4()
    {
        Restart();
        in_Iterations = 5;
        if_Iterations.text = in_Iterations.ToString();
        fl_Angle = 22.5f;
        if_Angle.text = fl_Angle.ToString();
        ch_Axiom1 = ch_F;
        if_Character1.text = ch_Axiom1.ToString();
        ch_Axiom2 = ch_X;
        if_Character2.text = ch_Axiom2.ToString();
        st_Rule1 = "F-[[X]+X]+F[+FX]-X";
        if_Rule1.text = st_Rule1;
        st_Rule2 = "FF";
        if_Rule2.text = st_Rule2;
        st_Axiom = "F";
        if_StartAxiom.text = st_Axiom;
        OnGenerator();
    }

    /// <summary>
    /// Calles the Restart method, changes the data or the iteration and pastes them in the appropriate InputField, and then calles the OnGenerator method.
    /// </summary>
    public void NextIteration()
    {
        Restart();
        if (in_Iterations < 7)
        {
            in_Iterations++;
            if_Iterations.text = in_Iterations.ToString(); 
        }
        OnGenerator();
    }

    /// <summary>
    /// Calles the Restart method, changes the data or the iteration and pastes them in the appropriate InputField, and then calles the OnGenerator method.
    /// </summary>
    public void PreviousIteration()
    {
        Restart();
        if (in_Iterations > 1)
        {
            in_Iterations--;
            if_Iterations.text = in_Iterations.ToString(); 
        }
        OnGenerator();
    }

    /// <summary>
    /// Destroys every object in the scene (they all are in the List) and resets the spawner's position.
    /// </summary>
    public void Restart()
    {
        foreach (GameObject i in l_AllMetres)
        {
            Destroy(i);
        }

        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Does what the Restart method does, but also resets the data in the InputFields.
    /// </summary>
    public void ResetScreen()
    {
        foreach (GameObject i in l_AllMetres)
        {
            Destroy(i);
        }

        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        in_Iterations = 1;
        if_Iterations.text = in_Iterations.ToString();
        fl_Angle = 0f;
        if_Angle.text = fl_Angle.ToString();
        ch_Axiom1 = ch_F;
        if_Character1.text = ch_Axiom1.ToString();
        ch_Axiom2 = char.MinValue;
        if_Character2.text = ch_Axiom2.ToString();
        st_Rule1 = "";
        if_Rule1.text = st_Rule1;
        st_Rule2 = "";
        if_Rule2.text = st_Rule2;
        st_Axiom = "";
        if_StartAxiom.text = st_Axiom;
    }

    /// <summary>
    /// Initialises the list to record the objects in the scene.
    /// </summary>
    private void Start()
    {
        l_AllMetres = new List<GameObject>();
    }

    /// <summary>
    /// This is the core of the script. It generates the L-System.
    /// </summary>
    public void OnGenerator()
    {
        //Makes sure that if the text in the InputFields is not null, it modifies the data. Also, it prevents the iterations to be 0 or below and 8 or above.
        if (if_Iterations.text != "")
        {
            if (in_Iterations <= 0)
            {
                if_Iterations.text = "1";
                in_Iterations = int.Parse(if_Iterations.text);
            }

            if (in_Iterations >= 8)
            {
                if_Iterations.text = "7";
                in_Iterations = int.Parse(if_Iterations.text);
            }

            else if (in_Iterations > 0 && in_Iterations < 8)
            {
                in_Iterations = int.Parse(if_Iterations.text);
            }
        }

        //Makes sure that if the text in the InputFields is not null, it modifies the data.
        if (if_Character1.text != "")
        {
            ch_Axiom1 = char.Parse(if_Character1.text); 
        }

        //Makes sure that if the text in the InputFields is not null, it modifies the data. Also, if there is another character, it does the same to it and its rule.
        if (if_Rule1.text != "")
        {
            st_Rule1 = if_Rule1.text; 

            if (if_Character2.text != "")
            {
                ch_Axiom2 = char.Parse(if_Character2.text);

                st_Rule2 = if_Rule2.text;
            }
        }

        //Makes sure that if the text in the InputFields is not null, it modifies the data.
        if (if_Angle.text != "")
        {
            fl_Angle = float.Parse(if_Angle.text); 
        }

        //Makes sure that if the text in the InputFields is not null, it modifies the data.
        if (if_StartAxiom.text != "")
        {
            st_Axiom = if_StartAxiom.text;
        }

        //Initialises the stacks.
        s_qRot = new Stack<Quaternion>();
        s_v3Pos = new Stack<Vector3>();

        //Stores the original rules. Used in the replacement process and to restore the final rule once changes have been applied to it.
        st_Original1 = st_Rule1;
        st_Original2 = st_Rule2;

        //Stores the original characters. Used in the replacement process.
        AOld = ch_Axiom1;
        BOld = ch_Axiom2;

        //Makes sure that if the iteration is zero, it sets it to 1.
        if (in_Iterations <= 0)
        {
            in_Iterations = 1;
        }

        //If the rule to replace the second character is empty (so there is no second character), replace the final rule's "F"s with the rule itself. To avoid errors, it is first replaced with "A" and then "A" is replace with the rule.
        if (st_Rule2 == "")
        {
            //If the axiom is longer than one character and does not match the first character, the rule's "F"s become the axiom.
            if (st_Axiom != ch_Axiom1.ToString())
            {
                st_Rule1 = st_Rule1.Replace(ch_Axiom1.ToString(), ANew.ToString());

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Axiom);
            }

            if (in_Iterations == 1)
            {
                //Nothing happens
            }

            //If the iteration is greater than 1, replace the rule's "F"s with "A"s and then replace the "A"s with the original rule.
            if (in_Iterations == 2)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            }

            if (in_Iterations == 3)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            }

            if (in_Iterations == 4)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            }

            if (in_Iterations == 5)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            }

            if (in_Iterations == 6)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            }

            if (in_Iterations == 7)
            {
                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld, ANew);

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Original1);
            } 
        }

        //If there is a second character, replace first the "F"s with the other "F"s and then all the "F"s with the original rule.
        else if (st_Rule2 != "")
        {
            //If the axiom is longer than one character and does not match the first character, the rule's "F"s become the axiom.
            if (st_Axiom != ch_Axiom1.ToString())
            {
                st_Rule1 = st_Rule1.Replace(ch_Axiom1.ToString(), ANew.ToString());

                st_Rule1 = st_Rule1.Replace(ANew.ToString(), st_Axiom);
            }

            if (in_Iterations == 1)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 2)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 3)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 4)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 5)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 6)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }

            if (in_Iterations == 7)
            {
                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);

                st_Rule1 = st_Rule1.Replace(AOld.ToString(), st_Original2);

                st_Rule1 = st_Rule1.Replace(BOld.ToString(), st_Original1);
            }
        }

        //This is were the "drawing" of the L-System happens. First, check every character in the rule, which by now is all layed out and everything has been replaced.
        foreach (char i in st_Rule1)
        {
            //If there is a character in the rule that matches with the original character (Ex: if you find an F), move the spawner 1 metre above and instantiate a 1-metre object and add it to the list. 
            if (ch_Axiom1 == i)
            {
                gameObject.transform.position += transform.up;
                GameObject go_Temp;
                go_Temp = Instantiate(go_1Metre, transform.position, transform.rotation);
                l_AllMetres.Add(go_Temp);
            }

            //If there is a plus in the rule...
            if (i.ToString() == "+")
            {
                //... If the rule to replacee the "F"s does not exist, move the spawner above and rotate it by a positive angle.
                if (st_Rule2 == "")
                {
                    gameObject.transform.rotation = transform.rotation * Quaternion.Euler(fl_Angle, 0, 0); 
                }

                //... Otherwise more the spawner above and rotate it by a negative angle.
                else
                {
                    gameObject.transform.rotation = transform.rotation * Quaternion.Euler(-fl_Angle, 0, 0);
                }
            }

            //If there is a minus in the rule... Same thing as above.
            if (i.ToString() == "-")
            {
                if (st_Rule2 == "")
                {
                    gameObject.transform.rotation = transform.rotation * Quaternion.Euler(-fl_Angle, 0, 0); 
                }

                else
                {
                    gameObject.transform.rotation = transform.rotation * Quaternion.Euler(fl_Angle, 0, 0);
                }
            }

            //If there is a "[" in the rule, push the position and rotation of the spawner in the data stack. Here you are basically asking the data stack to remember where you were at this point in time.
            if (i.ToString() == "[")
            {
                s_v3Pos.Push(transform.position);
                s_qRot.Push(transform.rotation);
            }

            //If there is a "]" in the rule, first peek (look at the most recent position and rotation and apply it to the spawner), then pop (delete this data since we don't need them anymore, we already are where the data told us we were).
            if (i.ToString() == "]")
            {
                transform.position = s_v3Pos.Peek();
                transform.rotation = s_qRot.Peek();
                s_v3Pos.Pop();
                s_qRot.Pop();
            }
        }

        //Store the layed-out string in st_Formula. You never know.
        st_Formula = st_Rule1;

        //Replace the rule with the original innocent rule it was at the beginning, so the InputField does not go crazy and displays a 4 million characters string.
        st_Rule1 = st_Original1;

        //Finally, after the L-System has been drawn, position the camera in front of the spawner, which is generally either at the top of the L-System, or in the middle. From there, the user can move the camera to check the L-System out.
        Camera.main.transform.position = gameObject.transform.position + new Vector3(80, 0, 0);
    }
}