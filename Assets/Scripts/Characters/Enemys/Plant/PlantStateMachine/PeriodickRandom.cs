using UnityEngine;

namespace PlantStateMachine
{
    public static class PeriodickRandom
    {
        static float _timer = 0, _timerMax = 3;

        public static void SetRandomValueEvery_3_seconds(ref bool value)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timerMax)
            {
                _timer = 0;
                value = Random.Range(0, 2) == 0 ? true : false;
            }
        }

        public static void Reset()
        {
            _timer = 0;
        }
    }
}

