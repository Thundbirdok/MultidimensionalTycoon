namespace GameResources.UI.GameUI.Control.ResourcesQuota.Scripts
{
    using GameResources.Control.Economy.Resources.Scripts;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ResourceQuotaProgressView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resourceName;
        
        [SerializeField]
        private TextMeshProUGUI amountToQuota;
        
        [SerializeField]
        private Image fill;
        
        private const string DONE_TEXT = "Done";
        
        private IResourceHandler _resourceHandler;
        private int _quota;

        private bool _isInited;
        
        public void Set(IResourceHandler resourceHandler, int quota)
        {
            _resourceHandler = resourceHandler;
            _quota = quota;

            _isInited = true;

            _resourceHandler.OnChangedValue += UpdateQuota;
            
            SetTexts();
        }
        
        private void OnEnable()
        {
            if (_isInited == false)
            {
                return;
            }
            
            UpdateQuota();
        }

        private void OnDestroy()
        {
            if (_resourceHandler != null)
            {
                _resourceHandler.OnChangedValue -= UpdateQuota;
            }
        }

        private void SetTexts()
        {
            if (_isInited == false)
            {
                return;
            }
            
            resourceName.text = _resourceHandler.ResourceType.Key;
            
            UpdateQuota();
        }

        private void UpdateQuota()
        {
            if (_resourceHandler.Value >= _quota)
            {
                amountToQuota.text = DONE_TEXT;
                fill.fillAmount = 1f;
                
                return;
            }
            
            amountToQuota.text = _resourceHandler.Value + "/" + _quota;
            fill.fillAmount = (float)_resourceHandler.Value / _quota;
        }
    }
}
