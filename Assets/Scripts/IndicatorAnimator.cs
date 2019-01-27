using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum RepeatMode
    {
        Once, OnceAndBack, PingPong
    }

    public class IndicatorAnimator : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        public Color intendedColor;

        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // SIZE

        private float _sizeDuration;
        private Vector2 _startSize, _finishSize;
        private RepeatMode _sizeRepeatMode;
        public void AnimateToSize(Vector2 startSize, Vector2 finishSize, float duration, RepeatMode repeatMode)
        {
            _sizeDuration = duration;
            _startSize = startSize;
            _finishSize = finishSize;
            _sizeRepeatMode = repeatMode;
            
            StopAnimatingToSize();
            StartCoroutine(C_AnimateToSize());
        }

        public void StopAnimatingToSize()
        {
            StopCoroutine(C_AnimateToSize());
        }

        private IEnumerator C_AnimateToSize()
        {
            float startTime = Time.time;
            float timer = 0;
            while (timer <= _sizeDuration && transform != null)
            {
                timer = Time.time - startTime;
                transform.localScale = Vector2.Lerp(_startSize, _finishSize, timer / _sizeDuration);
                yield return 0;
            }
            switch (_sizeRepeatMode)
            {
                case RepeatMode.OnceAndBack:
                {
                    Vector2 tempSize = _startSize;
                    _startSize = _finishSize;
                    _finishSize = tempSize;
                    _sizeRepeatMode = RepeatMode.Once;
                    StartCoroutine(C_AnimateToSize());
                }
                    break;
                case RepeatMode.PingPong:
                {
                    Vector2 tempSize = _startSize;
                    _startSize = _finishSize;
                    _finishSize = tempSize;
                    StartCoroutine(C_AnimateToSize());
                }
                    break;
                default:
                    break;
            }
        }

        // ROTATION

       /* public void AnimateToRotation(Quaternion start, Quaternion finish, float t, RepeatMode mode)
        {
            Timing.KillCoroutines(animTag + AnimType.Rotation);
            Timing.RunCoroutine(C_AnimateToRotation(start, finish, t, mode), animTag + AnimType.Rotation);
        }

        private IEnumerator<float> C_AnimateToRotation(Quaternion start, Quaternion finish, float duration, RepeatMode mode)
        {
            float startTime = Time.time;
            float timer = 0;
            while (timer <= duration && transform != null)
            {
                timer = Time.time - startTime;
                transform.localRotation = Quaternion.Lerp(start, finish, timer / duration);
                yield return 0;
            }
            switch (mode)
            {
                case RepeatMode.OnceAndBack:
                    Timing.RunCoroutine(C_AnimateToRotation(finish, start, duration, RepeatMode.Once), animTag + AnimType.Rotation);
                    break;
                case RepeatMode.PingPong:
                    Timing.RunCoroutine(C_AnimateToRotation(finish, start, duration, RepeatMode.PingPong), animTag + AnimType.Rotation);
                    break;
                default:
                    break;
            }
        }*/

        // COLOR

        private float _colorDuration;
        private Color _startColor, _finishColor;
        private RepeatMode _colorRepeatMode;
        public void AnimateToColor(Color start, Color finish, float duration, RepeatMode mode)
        {
            StopCoroutine(C_AnimateToColor());

            _colorDuration = duration;
            _startColor = start;
            _finishColor = finish;
            _colorRepeatMode = mode;
            
            StartCoroutine(C_AnimateToColor());
        }

        private IEnumerator C_AnimateToColor()
        {
            float startTime = Time.time;
            float timer = 0;
            while (timer <= _colorDuration && _spriteRenderer != null)
            {
                timer = Time.time - startTime;
                _spriteRenderer.color = Color.Lerp(_startColor, _finishColor, timer / _colorDuration);
                yield return 0;
            }
            switch (_colorRepeatMode)
            {
                case RepeatMode.OnceAndBack:
                {
                    Color temp = _startColor;
                    _startColor = _finishColor;
                    _finishColor = temp;
                    _colorRepeatMode = RepeatMode.Once;
                    StartCoroutine(C_AnimateToColor());
                }
                    break;
                case RepeatMode.PingPong:
                {
                    Color temp = _startColor;
                    _startColor = _finishColor;
                    _finishColor = temp;
                    StartCoroutine(C_AnimateToColor());
                }
                    break;
                default:
                    break;
            }
        }

        public void StopAllAnimations()
        {
            StopCoroutine(C_AnimateToColor());
            StopCoroutine(C_AnimateToSize());
        }

        void OnDestroy()
        {
            StopAllAnimations();
        }
    }