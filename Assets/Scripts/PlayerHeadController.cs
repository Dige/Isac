using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerHeadController : MonoBehaviour
    {
        [SerializeField]
        private Sprite _headFront;
        public Sprite HeadFront
        {
            get { return _headFront; }
            set { _headFront = value; }
        }

        [SerializeField]
        private Sprite _headBack;
        public Sprite HeadBack
        {
            get { return _headBack; }
            set { _headBack = value; }
        }

        [SerializeField]
        private Sprite _headSideways;
        public Sprite HeadSideways
        {
            get { return _headSideways; }
            set { _headSideways = value; }
        }

        private SpriteRenderer _spriteRenderer;

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetHeadDirection(HeadDirection direction)
        {
            switch (direction)
            {
                case HeadDirection.Up:
                    _spriteRenderer.sprite = _headBack;
                    break;
                case HeadDirection.Down:
                    _spriteRenderer.sprite = _headFront;
                    break;
                case HeadDirection.Left:
                    _spriteRenderer.sprite = _headSideways;
                    if ((transform.parent.transform.localScale.x < 0 && transform.localScale.x > 0) ||
                        transform.parent.transform.localScale.x > 0 && transform.localScale.x < 0)
                        TransformHelpers.FlipX(gameObject);
                    break;
                case HeadDirection.Right:
                    _spriteRenderer.sprite = _headSideways;
                    if ((transform.parent.transform.localScale.x < 0 && transform.localScale.x < 0) ||
                        transform.parent.transform.localScale.x > 0 && transform.localScale.x > 0)
                        TransformHelpers.FlipX(gameObject);
                    break;
            }
        }

        public enum HeadDirection
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}
