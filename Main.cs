using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static vp_Inventory;

namespace ThiefCheatV
{
    public class Main : MonoBehaviour
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        Player player = FindObjectOfType<Player>();

        private bool MainMenu;
        public Rect Rt_Mainmenu = new Rect(0, 0, 400, 400);
        private Color backgroundColor;
        private int tab;

        private int moneyAdded = 0;
        private int levelAdded = 0;
        private int skillpointAdded = 0;
        private float speedAdded = 0;

        public string watermarkText = "ViniLog best coder?";
        public int fontSize = 20;
        public TextAnchor anchor = TextAnchor.LowerLeft;
        public float colorChangeDuration = 2.0f;
        public Color[] colors = { Color.red, Color.green, Color.blue };
        public float fpsUpdateInterval = 0.5f;

        private float _timer;
        private float _fpsTimer;
        private int _currentColorIndex;
        private Color _startColor;
        private Color _targetColor;
        private float _fps;

        public bool disablePolice = true;

        private void Awake()
        {
            backgroundColor = HexToColor("77b0bd");
        }

        private void OnGUI()
        {
            if (this.MainMenu)
            {
                GUI.backgroundColor = backgroundColor;
                this.Rt_Mainmenu = GUILayout.Window(1, this.Rt_Mainmenu, new GUI.WindowFunction(this.Menu_Hack), "VCom Team • Owner @ViniLog | Olesya best girl!", new GUILayoutOption[0]);
            }
            DrawWatermark();
        }

        private void DrawWatermark()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = TextColor;
            style.fontSize = fontSize;
            style.alignment = anchor;

            float margin = 10f;

            Rect rect;
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    rect = new Rect(margin, margin, 300f, 50f);
                    break;
                case TextAnchor.UpperRight:
                    rect = new Rect(Screen.width - 300f - margin, margin, 300f, 50f);
                    break;
                case TextAnchor.LowerLeft:
                    rect = new Rect(margin, Screen.height - 50f - margin, 300f, 50f);
                    break;
                case TextAnchor.LowerRight:
                    rect = new Rect(Screen.width - 300f - margin, Screen.height - 50f - margin, 300f, 50f);
                    break;
                default:
                    rect = new Rect(0, 0, Screen.width, Screen.height);
                    break;
            }

            GUI.Label(rect, WatermarkText, style);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                this.MainMenu = !this.MainMenu;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                this.MainMenu = !this.MainMenu;
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                this.MainMenu = !this.MainMenu;
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Loader._Unload();
            }

            UpdateWatermark();

            PoliceCarController[] policeCars = FindObjectsOfType<PoliceCarController>();

            foreach (PoliceCarController car in policeCars)
            {
                car.enabled = !disablePolice;

                car.gameObject.SetActive(!disablePolice);
            }

            PoliceArrivesHud[] policeArrivesHuds = FindObjectsOfType<PoliceArrivesHud>();
            foreach (PoliceArrivesHud hud in policeArrivesHuds)
            {
                hud.gameObject.SetActive(!disablePolice);
            }

            PoliceLights[] policeLights = FindObjectsOfType<PoliceLights>();
            foreach (PoliceLights lights in policeLights)
            {
                lights.enabled = !disablePolice;
            }

            ISMART[] iSMARTs = FindObjectsOfType<ISMART>();
            foreach (ISMART smart in iSMARTs)
            {
                smart.enabled = !disablePolice;
            }
        }

        private void UpdateWatermark()
        {
            _timer += Time.deltaTime;
            float t = Mathf.Clamp01(_timer / colorChangeDuration);
            Color currentColor = Color.Lerp(_startColor, _targetColor, t);

            _fpsTimer += Time.deltaTime;
            if (_fpsTimer >= fpsUpdateInterval)
            {
                _fps = Mathf.Round(1.0f / Time.deltaTime);
                _fpsTimer = 0f;
            }

            if (t >= 1f)
            {
                _timer = 0f;
                _startColor = _targetColor;
                _currentColorIndex = (_currentColorIndex + 1) % colors.Length;
                _targetColor = GetNextColor();
            }

            WatermarkText = watermarkText + " | FPS: " + _fps;
            TextColor = currentColor;
        }

        public string WatermarkText { get; private set; }
        public Color TextColor { get; private set; }

        private Color GetNextColor()
        {
            return colors[(_currentColorIndex + 1) % colors.Length];
        }

        private void Menu_Hack(int id)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(120));

            GUILayout.BeginVertical(GUILayout.Width(120));
            string[] tabNames = { "Player", "Time", "Police", "Info" };
            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Toggle(tab == i, tabNames[i], "Button", GUILayout.ExpandWidth(true)))
                {
                    tab = i;
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(300));
            GUILayout.Space(5);

            switch (tab)
            {
                case 0: // Игрок
                    GUILayout.Label("Add Money:", new GUILayoutOption[0]);
                    moneyAdded = int.Parse(GUILayout.TextField(moneyAdded.ToString(), new GUILayoutOption[0]));
                    if (GUILayout.Button("Add Current Money", new GUILayoutOption[0]))
                    {
                        PlayerInventory.cash += moneyAdded;
                    }
                    GUILayout.Label("Add Level:", new GUILayoutOption[0]);
                    levelAdded = int.Parse(GUILayout.TextField(levelAdded.ToString(), new GUILayoutOption[0]));
                    if (GUILayout.Button("Add Current Level", new GUILayoutOption[0]))
                    {
                        PlayerInventory.level += levelAdded;
                    }
                    GUILayout.Label("Add Skill Points:", new GUILayoutOption[0]);
                    skillpointAdded = int.Parse(GUILayout.TextField(skillpointAdded.ToString(), new GUILayoutOption[0]));
                    if (GUILayout.Button("Add Current Skill Points", new GUILayoutOption[0]))
                    {
                        PlayerInventory.skillPoints += skillpointAdded;
                    }
                    if (GUILayout.Button("Add items", new GUILayoutOption[0]))
                    {
                        foreach (ItemAsset item in GameController.instance.itemsForCheat)
                        {
                            PlayerInventory.instance.AddItem(item, 100f, "cheatitem" + UnityEngine.Random.Range(111111, 999999), string.Empty);
                        }
                    }
                    if (GUILayout.Button("Complete the mission", new GUILayoutOption[0]))
                    {
                        GameController.instance.RaportStoryDone(GameController.instance.currentStory);
                    }
                    GUILayout.Space(10);
                    GUILayout.Label("It only works in the house!", new GUILayoutOption[0]);
                    if (GUILayout.Button("Get new pickup", new GUILayoutOption[0]))
                    {
                        UnityEngine.Object.FindObjectOfType<CarsDisassembleController>().LoadCarByName("Pickup new car 1");
                    }
                    if (GUILayout.Button("Get old pickup", new GUILayoutOption[0]))
                    {
                        UnityEngine.Object.FindObjectOfType<CarsDisassembleController>().LoadCarByName("Pickup old car");
                    }
                    break;
                case 1: // Время
                    GUILayout.Label("It only works in the house!", new GUILayoutOption[0]);
                    GUILayout.Space(10);
                    if (GUILayout.Button("Day time (8:00)", new GUILayoutOption[0]))
                    {
                        WeatherController.instance.tenkoku.setHour = 8;
                        WeatherController.instance.tenkoku.currentHour = 8;
                        WeatherController.instance.tenkoku.currentMinute = 0;
                        foreach (ReflectionProbeUpdate reflectionProbeUpdate4 in WeatherController.instance.reflections)
                        {
                            if (reflectionProbeUpdate4.followPlayer)
                            {
                                reflectionProbeUpdate4.ForceRefreshNow();
                            }
                        }
                    }
                    if (GUILayout.Button("Evening time (18:00)", new GUILayoutOption[0]))
                    {
                        WeatherController.instance.tenkoku.setHour = 18;
                        WeatherController.instance.tenkoku.currentHour = 18;
                        WeatherController.instance.tenkoku.currentMinute = 0;
                        foreach (ReflectionProbeUpdate reflectionProbeUpdate5 in WeatherController.instance.reflections)
                        {
                            if (reflectionProbeUpdate5.followPlayer)
                            {
                                reflectionProbeUpdate5.ForceRefreshNow();
                            }
                        }
                    }
                    if (GUILayout.Button("Noon time (12:00)", new GUILayoutOption[0]))
                    {
                        WeatherController.instance.tenkoku.setHour = 12;
                        WeatherController.instance.tenkoku.currentHour = 12;
                        WeatherController.instance.tenkoku.currentMinute = 0;
                        foreach (ReflectionProbeUpdate reflectionProbeUpdate3 in WeatherController.instance.reflections)
                        {
                            if (reflectionProbeUpdate3.followPlayer)
                            {
                                reflectionProbeUpdate3.ForceRefreshNow();
                            }
                        }
                    }
                    if (GUILayout.Button("4:00", new GUILayoutOption[0]))
                    {
                        WeatherController.instance.tenkoku.setHour = 4;
                        WeatherController.instance.tenkoku.currentHour = 4;
                        WeatherController.instance.tenkoku.currentMinute = 0;
                        foreach (ReflectionProbeUpdate reflectionProbeUpdate2 in WeatherController.instance.reflections)
                        {
                            if (reflectionProbeUpdate2.followPlayer)
                            {
                                reflectionProbeUpdate2.ForceRefreshNow();
                            }
                        }
                    }
                    if (GUILayout.Button("Night time (00:00)", new GUILayoutOption[0]))
                    {
                        WeatherController.instance.tenkoku.setHour = 24;
                        WeatherController.instance.tenkoku.currentHour = 24;
                        WeatherController.instance.tenkoku.currentMinute = 0;
                        foreach (ReflectionProbeUpdate reflectionProbeUpdate in WeatherController.instance.reflections)
                        {
                            if (reflectionProbeUpdate.followPlayer)
                            {
                                reflectionProbeUpdate.ForceRefreshNow();
                            }
                        }
                    }
                    break;
                case 2: // Полиция
                    disablePolice = GUILayout.Toggle(disablePolice, "Disable Police", new GUILayoutOption[0]);
                    if (GUILayout.Button("Add star 1", new GUILayoutOption[0]))
                    {
                        PlayerController.instance.isPoliceStars = 1;
                        PlayerController.instance.CallPolice();
                    }
                    if (GUILayout.Button("Add star 2", new GUILayoutOption[0]))
                    {
                        PlayerController.instance.lastSeenPosition = PlayerController.instance.GetPlayerPosition();
                        PlayerController.instance.isPoliceStars = 2;
                        PlayerController.instance.CallPolice();
                    }
                    break;

                case 3: // Информация
                    GUILayout.Label("Author: VCom Team • Owner @ViniLog", new GUILayoutOption[0]);
                    GUILayout.Label("Olesya the best girl!", new GUILayoutOption[0]);
                    GUILayout.Label("This modification is only available on the Playground.", new GUILayoutOption[0]);
                    GUILayout.Label("Copyright © VCom Team 2025. All rights reserved.", new GUILayoutOption[0]);
                    GUILayout.Space(5);
                    GUILayout.Label("Testers:", new GUILayoutOption[0]);
                    GUILayout.Label("@tualet1883", new GUILayoutOption[0]);
                    GUILayout.Label("@ViniLog", new GUILayoutOption[0]);
                    GUILayout.Space(10);
                    if (GUILayout.Button("Help", new GUILayoutOption[0]))
                    {
                        string url = "https://t.me/ViniLog";

                        try
                        {
                            Application.OpenURL(url);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError("Произошла ошибка при открытии ссылки: " + ex.Message);
                        }
                    }
                    if (GUILayout.Button("Our website", new GUILayoutOption[0]))
                    {
                        string url = "https://vcom-team.netlify.app/";

                        try
                        {
                            Application.OpenURL(url);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError("Произошла ошибка при открытии ссылки: " + ex.Message);
                        }
                    }
                    break;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        private void Start()
        {
            _timer = 0f;
            _fpsTimer = 0f;
            _currentColorIndex = 0;
            _startColor = colors[0];
            _targetColor = GetNextColor();
            _fps = 0f;
        }

        private Color HexToColor(string hex)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString("#" + hex, out color);
            return color;
        }
    }
}

// Исходник был опубликован на GitHub by ViniLog❤️