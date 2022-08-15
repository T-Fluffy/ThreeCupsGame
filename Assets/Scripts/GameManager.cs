using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static int vitess=0;
    public TMP_Text Message;
    public GameObject[] cups; 
    public Image ball; 
    public float MixerUp;
    bool finishedRevealing=false;
    public int rounds = 3;//rounds to mix
    public Sprite standing, laying;
    void Start()
    { 
        Message.text = "Search for the ball !";
        ball.enabled = false;
        StartCoroutine(CupMixer(0.3f));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("UI");
        }
    }
    public IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);  

    }
    public IEnumerator CupMixer(float wfs=0.3f)
    {
        Debug.Log(vitess);
        Vector3[] ts = new Vector3[cups.Length]; 
        for (int i = 0; i < cups.Length; i++) ts[i] = cups[i].transform.position.Copy();
        foreach (GameObject c in cups) StartCoroutine(RevealCup(c)); 
        yield return new WaitForSecondsRealtime(1);//reveal the cups
        for (int i = 0; i < rounds; i++)
        {
            Vector3[] randoms = ts.Randomize().Copy();
            foreach (GameObject c in cups)
            {
                StartCoroutine(c.TranslateOverTime(vitess, // el vitesse en ms 
                                                   Vector3.up * MixerUp)); 
                c.Button().interactable = false; 
            }
            yield return new WaitForSecondsRealtime(wfs);
            for (int k = 0; k < ts.Length; k++) 
            { 
                Vector3 movement = new Vector3(randoms[k].x - cups[k].transform.position.x, -MixerUp);
                StartCoroutine(cups[k].TranslateOverTime(vitess, movement)); 
            }
            yield return new WaitForSecondsRealtime(wfs);
        }
        foreach (GameObject c in cups) c.Button().interactable = true;
    }
   
    public IEnumerator RevealCup(GameObject cup)
    {
        finishedRevealing = false; 
        cup.SetActive(true); 
        cup.GetComponent<Image>().sprite = laying;
        ball.enabled = true;
        yield return new WaitForSecondsRealtime(1); 
        cup.GetComponent<Image>().sprite = standing;
        ball.enabled = false;
        finishedRevealing = true;
    }
    public void ClickedCup(GameObject g)
    {
        StartCoroutine(RevealCup(g)); 
        if (g.HasComponentInChildren<Image>()) 
        {
            Message.text = " Ball found !! nice !";
            StartCoroutine(CounterAnimation(Color.green));
            StartCoroutine(wait());
        }
        else 
        {
            Message.text = " Nop !! try again !";
            StartCoroutine(CounterAnimation(Color.red));
            StartCoroutine(wait());
        }
    }
    public IEnumerator CounterAnimation(Color c) 
    { 
        Message.color = c; 
        yield return new WaitForSeconds(2);
        Message.color = Color.white;
        SceneManager.LoadScene("main");
    }
    
}
public static class Configurations
{
    public static Button Button(this GameObject g) => g.GetComponent<Button>();
    public static Vector3[] Positions(this GameObject[] gs)
    {
        List<Vector3> vs = new List<Vector3>(); 
        foreach (GameObject g in gs) 
        { 
            Vector3 v = g.transform.position; 
            vs.Add(new Vector3(v.x, v.y, v.z)); 
        }
        return vs.ToArray();
    }
    public static T[] Randomize<T>(this T[] arr)
    {
        List<T> ts = new List<T>(); 
        List<int> index = new List<int>(), ni = new List<int>();//ni means new indexes
        for (int i = 0; i < arr.Length; i++) index.Add(i);
        while (index.Count > 0)
        {
            int ind = Random.Range(0, index.Count);
            ni.Add(index[ind]);
            index.RemoveAt(ind);
        }
        foreach (int n in ni) ts.Add(arr[n]); 
        return ts.ToArray();
    }
    public static bool HasComponentInChildren<T>(this GameObject g, bool ParentHasToo = true) 
    { 
        try { 
            T[] x = g.GetComponentsInChildren<T>();
            if (x == null || x.Length < 2 && ParentHasToo) int.Parse("sd");
            return true;
        } catch 
        { 
            return false; 
        } 
    }
    public static Vector3 Copy(this Vector3 v) => new Vector3(v.x, v.y, v.z);
    public static Vector3[] Copy(this Vector3[] vs)
    { 
        List<Vector3> nv = new List<Vector3>(); 
        foreach (Vector3 v in vs) nv.Add(new Vector3(v.x, v.y, v.z)); 
        return nv.ToArray(); 
    }
    public static void SetActive(this GameObject[] gs, bool b) 
    { 
        foreach (GameObject g in gs) g.SetActive(b); 
    }
    public static IEnumerator TranslateOverTime(this GameObject g, int milliseconds, Vector3 movement)
    {
        for (int i = 0; i < milliseconds; i++)
        {
            g.transform.Translate(movement / milliseconds, Space.World); 
            yield return new WaitForSecondsRealtime(.001f);
        }
    }
    public static IEnumerator TranslateOverTimes1(this GameObject g, Vector3 target, int milliseconds)
    {
        Vector3 movement = target - g.transform.position;
        for (int i = 0; i < milliseconds; i++)
        {
            g.transform.Translate(target / milliseconds, Space.World);
            yield return new WaitForSecondsRealtime(.001f);
        }
    }
}
