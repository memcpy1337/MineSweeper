using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MineSweeper : MonoBehaviour
{
    public GameObject[] button = new GameObject[64];
    public int[] mines = new int[8];
    public bool[] visited = new bool[64];
    public GameObject losed, win, prefab;
   
    public int[,] GamePool = {
        {1, 2, 3, 4, 5, 6, 7, 8 },
        {9, 10, 11, 12, 13, 14, 15, 16 },
        {17, 18, 19, 20, 21, 22, 23, 24 },
        {25, 26, 27, 28, 29, 30, 31, 32 },
        {33, 34, 35, 36, 37, 38, 39, 40 },
        {41, 42, 43, 44, 45, 46, 47, 48 },
        {49, 50, 51, 52, 53, 54, 55, 56 },
        {57, 58, 59, 60, 61, 62, 63, 64 },
    };
    // Start is called before the first frame update
    void Start()
    {
        
        for (int b = 0; b < mines.Length; b++)
        {
            mines[b] = -1;
        }
        //Random MINED buttons
        int x = 0;
        System.Random rnd = new System.Random();
        while (x < mines.Length)
        {
            int k = rnd.Next(1, 65);

            while (mines.Contains(k))
            {
                k = rnd.Next(1, 65);
            }
            mines[x] = k;
            x++;
        }
     

    }






    //public void OnPointerClick(PointerEventData eventData)
    // {

    // }
    public void Reset1()
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].GetComponent<Image>().color = Color.white;
            button[i].GetComponent<Button>().interactable = true;
            button[i].GetComponentInChildren<Text>().enabled = false;
            button[i].GetComponentInChildren<Text>().text = "";

        }
        for (int y = 0; y < visited.Length; y++)
        {
            visited[y] = false;
        }

        Start();

       
    }
    public void ClickButton(Button button1)
        {
      
        Debug.Log(button1.name);
        int button_id = Convert.ToInt32(button1.name);
        button_id += -1;

        if (!(CheckLose(button_id)))
        {

            GameLoop(button_id);

        }else
        {
            Debug.Log("You lose");
            losed.SetActive(true);
            for (int check = 0; check < button.Length; check++)
            {
                button[check].GetComponent<Button>().interactable = false;
            }
        }

      
        }
    bool CheckLose(int button_id)
    {
        for (int i = 0; i < mines.Length; i++)
        {
            //Debug.Log("ТЫ НАЖАЛ НА КНОПКУ НОМЕР " + button[button_id].name);
            if (button[button_id].name == Convert.ToString(mines[i]))
            {
               
                for (int y = 0; y < mines.Length; y++)
                {
                    button[mines[y] - 1].GetComponentInChildren<Text>().enabled = true;
                    button[mines[y] - 1].GetComponentInChildren<Text>().text = "*";
                    button[mines[y] - 1].GetComponent<Image>().color = Color.red;
                    //Debug.Log("ЗАКРАСИЛИ КНОПКУ: " + button[mines[y] - 1].name);
                }
                return true;
                
            }
        }
        return false;
    }

    void GameLoop(int button_id)
    {
        int end_button_id = button_id + 1;
        for (int i = 0; i < 8; i++) //НОМЕР СТРОКИ В ДВУМЕРНОМ МАССИВЕ
        {
            for (int y = 0; y < 8; y++) //НОМЕР ЭЛЕМЕНТА В ЭТОЙ СТРОКЕ
            {
                if (GamePool[i, y] == end_button_id) //НАШЛИ НАШУ КНОПКУ В ЭТОМ ДВУМЕРНОМ МАССИВЕ, ПОЛУЧИЛИ КООРДИНАТЫ
                {
                    int nearby = CheckNearbyMine(i, y);
                    if (nearby != 0) //Если рядом с нажатой кнопкой есть мина
                    {
                        Debug.Log("Количество мин рядом: " + nearby);
                        button[button_id].GetComponentInChildren<Text>().enabled = true;
                        button[button_id].GetComponent<Image>().color = Color.yellow;
                        button[button_id].GetComponent<Button>().interactable = false;
                        button[button_id].GetComponentInChildren<Text>().text = Convert.ToString(nearby);

                       
                    }
                    else
                    {
                        int index_i = i;
                        int index_y = y;
                        Debug.Log("Рядом с этой кнопкой мин не обнаружено");
                        button[GamePool[index_i, index_y]-1].GetComponent<Button>().interactable = false;
                        button[GamePool[index_i, index_y]-1].GetComponent<Image>().color = Color.gray;
                        visited[GamePool[index_i, index_y] - 1] = true;
                        
                        Open(index_i, index_y);
                                   
                    }


                }



            }
                

        }
        int checker = 0;
        for (int bi = 0; bi < button.Length; bi++)
        {
            if (button[bi].GetComponent<Image>().color == Color.white)
            {
                checker++;

            }
        }
        if (checker == 8)
        {
            win.SetActive(true);
        }
    }
    public void SetActive1()
    {
        this.gameObject.SetActive(true);
    }

    void Open(int i, int y) //рекурсивно_открываем
    {
        int index_i, index_y;
      
        for (int b = 0; b < 8; b++)
        {
            
            switch (b)
            {

                case 0:
                    index_i = i - 1;
                    index_y = y - 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                               
                                Open(index_i, index_y); 

                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));
                              
                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;

                case 1:
                    index_i = i - 1;
                    index_y = y;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                              
                                Open(index_i, index_y);

                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 2:
                    index_i = i - 1;
                    index_y = y + 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);

                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 3:
                    index_i = i;
                    index_y = y - 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);
                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 4:
                    index_i = i;
                    index_y = y + 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);
                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 5:
                    index_i = i + 1;
                    index_y = y - 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);
                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 6:
                    index_i = i + 1;
                    index_y = y;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);

                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
                case 7:
                    index_i = i + 1;
                    index_y = y + 1;
                    try
                    {
                        if (visited[GamePool[index_i, index_y] - 1] == false)
                        {

                            if (CheckNearbyMine(index_i, index_y) == 0)
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.gray;
                                visited[GamePool[index_i, index_y] - 1] = true;
                                Open(index_i, index_y);
                            }
                            else
                            {

                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().enabled = true;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Image>().color = Color.yellow;
                                button[GamePool[index_i, index_y] - 1].GetComponent<Button>().interactable = false;
                                button[GamePool[index_i, index_y] - 1].GetComponentInChildren<Text>().text = Convert.ToString(CheckNearbyMine(index_i, index_y));



                            }
                            visited[GamePool[index_i, index_y] - 1] = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex);
                    }
                    break;
            }


        }


    }


    int CheckNearbyMine(int i, int y)
    {
        int summ_mines = 0;
        for (int b = 0; b < 8; b++) //ЧЕКАЕМ ВСЕХ СОСЕДЕЙ
        {
            switch (b)
            {
                case 0:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i - 1, y - 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 1:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i - 1, y] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 2:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i - 1, y + 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 3:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i, y - 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 4:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i, y + 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 5:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i + 1, y - 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 6:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i + 1, y] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
                case 7:
                    for (int m = 0; m < mines.Length; m++)
                    {
                        try
                        {
                            if (GamePool[i + 1, y + 1] == mines[m])
                            {
                                summ_mines++;
                                Debug.Log("Сосед мина этой клетки это номер: " + mines[m] + "Координаты: " + i + ":" + y);
                                break;
                            }
                        }
                        catch //НЕТ СОСЕДА С ЭТОЙ СТОРОНЫ
                        {
                            break;
                        }
                    }
                    break;
            }

        }

        return summ_mines;
    }
    
}
