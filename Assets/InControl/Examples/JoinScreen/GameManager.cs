using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

namespace JoinScreenExample
{
    public class GameManager : MonoBehaviour
    {
        public List<InputDevice> Controllers = new List<InputDevice>();

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (this != _instance)
                    Destroy(gameObject);
            }
        }
    }
}
