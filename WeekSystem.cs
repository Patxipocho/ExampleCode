using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//Script  Clock and  Weekly calendar to do events in certain days.
public class WeekSystem : MonoBehaviour
{
    public static WeekSystem instance;

    public Client_Spawner spawner;

    
    private MenuManager menuManager;
    
    //UI
    [Header("UI CANVAS"), Space(20)]
    public TextMeshProUGUI hourText;
    public TextMeshProUGUI minuteText;

    //Events
    public delegate void OnDayEnd();
    public static event OnDayEnd onDayEnd;

    public bool gameStarted;

   
    public int week;
    [Range(8, 18)]
    public int hour;
    public int endHour;
    
    public int startingHour;
    private float minute;

    //Every "Tick" the time advance a certain "time" ( in this case every tick the time advance 10 min)
    public float timeBetweenTicks;
    private float currentTime;
   
    //How much time to advance every tick
    public float minutesToAdvance;
    private bool finishDay;

    public DayOfWeek dayOfWeek;


    private void Awake()
    {
        spawner = GetComponent<Client_Spawner>();
        DontDestroyOnLoad(this);
        minuteText = GameObject.FindGameObjectWithTag("Canvas").gameObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        hourText = GameObject.FindGameObjectWithTag("Canvas").gameObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();


        //singleton
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);

        }

    }



    private void Start()
    {
        
        hour = startingHour;
        hourText.text = hour.ToString();
        minuteText.text = minute.ToString();


        if (dayOfWeek == DayOfWeek.Null)
        {
            dayOfWeek = DayOfWeek.Lunes;
        }


    }


    private void Update()
    {
       
            CheckIfDayHasFinished();
        
        
    }

    //WeekSystem
    #region WeekSystem

      
    private void CheckIfDayHasFinished()
    {
        if (!finishDay)
        {
            //Contador
            currentTime += Time.deltaTime;

            if (currentTime >= timeBetweenTicks)
            {
                currentTime = 0;
                AdvanceMinutes();
            }
        }
        //Cuando acaba el dia iniciar el evento correspondiente
        if (finishDay)
        {
           if(onDayEnd != null)
            {
                onDayEnd();
            }
        }
    }

   //Reloj
    public void AdvanceMinutes()
    {
        minute += minutesToAdvance;
        if (minute >= 60)
        {
            minute = 0;
            AdvanceHour();

        }
        UpdateTimeText();
    }
    public void AdvanceHour()
    {
        hour++;
        if (hour >= endHour)
        {
            hour = endHour;
            finishDay = true;
            spawner._canSpawn = false;
        }
    }

    //Cuando acabe y el jugador quiera, avanza el dia.
    public void AdvanceDay()
    {
        hour = 0;
        minute = 0;
        UpdateTimeText();
        switch (dayOfWeek)
        {
            case DayOfWeek.Null:
                dayOfWeek = DayOfWeek.Lunes;
                finishDay = false;
                break;
            case DayOfWeek.Lunes:
                dayOfWeek = DayOfWeek.Martes;
                finishDay = false;
                break;
            case DayOfWeek.Martes:
                dayOfWeek = DayOfWeek.Miercoles;
                finishDay = false;
                break;
            case DayOfWeek.Miercoles:
                dayOfWeek = DayOfWeek.Jueves;
                finishDay = false;

                break;
            case DayOfWeek.Jueves:
                dayOfWeek = DayOfWeek.Viernes;
                finishDay = false;

                break;
            case DayOfWeek.Viernes:
                dayOfWeek = DayOfWeek.Weekend;
                finishDay = false;

                break;
            case DayOfWeek.Weekend:
                dayOfWeek = DayOfWeek.Lunes;
                finishDay = false;
                week++;
                break;
        }
      
        SceneManager.LoadScene(0);

    }

    public void UpdateTimeText()
    {
        hourText.text = hour.ToString("F0") + ":";
        minuteText.text = minute.ToString();
    }
    #endregion


}


public enum DayOfWeek
{
    Null = 0,
    Lunes = 1,
    Martes = 2,
    Miercoles = 3,
    Jueves = 4,
    Viernes = 5,
    Weekend = 6,

}