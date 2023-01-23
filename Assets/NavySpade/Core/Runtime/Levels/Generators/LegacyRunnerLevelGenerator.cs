using System.Collections.Generic;
using System.Linq;
using EventSystem.Runtime.Core.Dispose;
using EventSystem.Runtime.Core.Managers;
using Main.Levels.Data;
using Main.Levels.Generators;
using NavySpade.Core.Runtime.Player.Logic;
using NavySpade.Modules.Extensions.CsharpTypes;
using UnityEngine;

namespace Main
{
    public class LegacyRunnerLevelGenerator : LevelGenerator<LegacyRunnerLevelData>
    {
        [SerializeField] private Transform _root = default;
        [SerializeField] private float _createDistance = 50f;
        [SerializeField] private float _destroyDistance = 50f;
        [SerializeField] private float _step = 10f;
        private Transform FinishPoint;
        [SerializeField] private List<GameElement> _objectsInGame = new List<GameElement>();

        private float PlayerPosition => _player!=null ? _player.transform.position.z : 0;
        private LegacyRunnerLevelData _data;
        private LevelElementsData e_data;
        private SinglePlayer _player;
        private void Awake()
        {
          //  EventManager.Add<LevelData>("Initialize", Initialize); ???
            EventManager.Add(GenerateEnumEM.Generate, Generate).AddTo(_disposal);
            EventManager.Add(GenerateEnumEM.Update, UpdateInfinity).AddTo(_disposal);
            EventManager.Add(MainEnumEvent.Clear, ClearLevel).AddTo(_disposal);
            EventManager.Add<SinglePlayer>(GenerateEnumEM.SetPlayer, SetPlayer).AddTo(_disposal);
            EventManager.Add<Transform>(MainEnumEvent.SetFinishPoint, SetFinishPoint).AddTo(_disposal);
        }

        private void SetPlayer(SinglePlayer player)
        {
            _player = player;
        }
        public void SetFinishPoint(Transform point)
        {
            FinishPoint = point;
        }
        private void Initialize(LegacyRunnerLevelData data)
        {
            _data = data;
            e_data = _data.LevelElements;
        }
        public void Generate()
        {
            Camera.main.GetComponent<Skybox>().material = _data.view.Sky.Material;
            
            CreateElement(e_data.startElement, Vector3.zero);

            var countSteps = e_data.randomOrder ? _createDistance / _step : e_data.gameElements.Count + 1;
            var distance = e_data.startElement.Size.z / 2f;

            for (var i = 0; i < countSteps; i++)
            {
                var isFinish = e_data.randomOrder && Mathf.Abs(distance) >= _data.distance
                               || e_data.randomOrder == false && i == countSteps - 1;

                var prefab = isFinish ? e_data.finishElement : GetNextElement(i);

                var lastPositionZ = distance + prefab.Size.z / 2f;
                var position = new Vector3(0, 0, lastPositionZ);
                var element = CreateElement(prefab, position);

                distance += prefab.Size.z;

                if (isFinish)
                {
                    EventManager.Invoke(MainEnumEvent.SetFinishPoint, element.Point);
                    return;
                }
            }
        }

        public void UpdateInfinity()
        {
            if (_objectsInGame[0].transform.position.z > PlayerPosition + _destroyDistance)
            {
                DestroyFirst();
            }

            var lastObject = _objectsInGame.Last();

            if (lastObject.transform.position.z > PlayerPosition - _createDistance && FinishPoint)
            {
                var distance = lastObject.transform.position.z - lastObject.Size.z / 2f;
                var isFinish = false;
                GameElement prefab;

                if (Mathf.Abs(distance) >= _data.distance - 1f)
                {
                    prefab = e_data.finishElement;
                    isFinish = true;
                }
                else
                {
                    prefab = e_data.gameElements.RandomElement();
                }

                var position = new Vector3(0, 0, distance);
                var element = CreateElement(prefab, position);

                if (isFinish)
                {
                    EventManager.Invoke(MainEnumEvent.SetFinishPoint, element.Point);
                }
            }
        }

        public void ClearLevel()
        {
            foreach (var o in _objectsInGame)
            {
                Destroy(o.gameObject);
            }

            _objectsInGame.Clear();
            EventManager.Invoke<Transform>(MainEnumEvent.SetFinishPoint, null);
        }

        private GameElement CreateElement(GameElement prefab, Vector3 position)
        {
            var element = Instantiate(prefab, _root);
            element.transform.SetPositionAndRotation(position, Quaternion.identity);

            _objectsInGame.Add(element);

            return element;
        }

        private GameElement GetNextElement(int index)
        {
            GameElement element;
            element = e_data.randomOrder ? e_data.gameElements.RandomElement() : e_data.gameElements[index];
            return element;
        }

        private void DestroyFirst()
        {
            var element = _objectsInGame[0];
            _objectsInGame.Remove(element);
            Destroy(element.gameObject);
        }

        private EventDisposal _disposal = new EventDisposal();
        void OnDestroy()
        {
            _disposal.Dispose();
        }

        protected override void OnGenerated(LegacyRunnerLevelData dataBase)
        {
            
        }

        protected override void OnCleanUp()
        {
            
        }
    }
}