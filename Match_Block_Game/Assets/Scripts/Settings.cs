using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Settings : MonoBehaviour
{
    public InputField row;
    public InputField column;
    public InputField colour;
    public InputField A;
    public InputField B;
    public InputField C;
    public Text errorMessageText;
    public GameObject PauseMenu;
    public int[] def;

    
    private void Start()
    {
        ManagerData def = GameManager.Instance.GetManager();

        row.text = def.Rows.ToString();
        column.text = def.Columns.ToString();
        colour.text = def.Colours.ToString();
        A.text = def.A.ToString();
        B.text = def.B.ToString();
        C.text = def.C.ToString();

    }
    public void Open(GameObject obj)
    {
        obj.SetActive(true);
    }
    public void Close(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void Save_Restart()
    {
        int r = int.Parse(row.text);
        int c = int.Parse(column.text);
        int col = int.Parse(colour.text);
        int a = int.Parse(A.text);
        int b = int.Parse(B.text);
        int cc = int.Parse(C.text);



        /*if (r < 0 || r > 10 || c < 0 || c > 10)
        {
            errorMessageText.text = "Sat�r ve s�tun de�erleri 0 ile 10 aras�nda olmal�!";
        }
        else if (col <= 0 || col > 6)
        {
            errorMessageText.text = "Renk de�eri 1 ile 6 aras�nda olmal�!";
        }
        else if (a <= 0)
        {
            errorMessageText.text = "A de�eri 0'dan b�y�k olmal�!";
        }
        else if (cc <= b)
        {
            errorMessageText.text = "C, B'den b�y�k olmal�!";
        }
        else if (b <= a)    
        {
            errorMessageText.text = "B, A'dan b�y�k olmal�!";
        }*/  
        // this part made editor crash?

        if (r < 2 || r > 10 || c < 2 || c > 10 || col <= 0 || col > 6 || a <= 0 || cc <= b || b <= a)
            return;

        else
        {
            GameManager.Instance.ClearBoard();
            
            GameManager.Instance.SetManager(new ManagerData(r, c, col, a, b, cc));
            errorMessageText.text = "";
            Close(PauseMenu);
            GameManager.Instance.Start();
        }
    }

}
