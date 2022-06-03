using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class Card : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Canvas canvas;
        protected float MAX_TIME = 3f;
        [SerializeField]
        protected float counter = 0f;
        [SerializeField]
        protected GameObject player;
        protected string range_prefab_path;
        protected Object range_prefab;
        protected GameObject range_gameobject;

        protected string effect_prefab_path;
        protected Object effect_prefab;
        protected GameObject effect_gameobject;

        protected void InitializeCard(float _MAX_TIME, string effect_path, string range_path){
            canvas = GameObject.Find("Battle Room Menu Canvas").GetComponent<Canvas>();
            MAX_TIME = _MAX_TIME;
            effect_prefab_path = effect_path;
            range_prefab_path = range_path;
        }

        void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData){
            OnDarag(eventData);
        }

        void IEndDragHandler.OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData){
            EndDrag(eventData);
        }

        public void OnDarag(BaseEventData data){

            if(counter != 0f)
                return;
            Debug.Log("正在拖動");
            PointerEventData pointerData = (PointerEventData)data;

            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                pointerData.position,
                canvas.worldCamera,
                out position
            );

            transform.position = canvas.transform.TransformPoint(position);
            transform.localPosition = new Vector2(0f, transform.localPosition.y);
            ShowRange();
        }

        public void EndDrag(BaseEventData data){
            Debug.Log("取消拖動");
            CloseRange();
            if(player.GetComponent<PlayerManager>().State == PlayerState.Walk)
                CardEffect();
            transform.localPosition = new Vector2(0f, 0f);
        }

        protected void TimeCounter(){
            if(counter == 0f)
                return;
            counter = counter - (1f *Time.deltaTime);
            if(counter < 0f)
                counter = 0f;
        }

        protected virtual void CardEffect(){
            if(transform.localPosition.y > 50f){
                Debug.Log("發動效果");
                counter = MAX_TIME;
            }
            else if(transform.localPosition.y < -50f){
                Debug.Log("切換屬性");
            }
        }

        protected void ShowRange(){
            if(range_prefab == null){
                range_prefab = Resources.Load(range_prefab_path, typeof(GameObject));
            }

            if(range_gameobject == null && range_prefab != null)
            {
                range_gameobject = Instantiate(range_prefab, player.transform.position, player.transform.rotation, player.transform) as GameObject;
                // range_gameobject.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }

        protected void CloseRange(){
            if(range_gameobject != null)
            {
                Destroy(range_gameobject);
            }
        }

        public void SettingPlayer(GameObject _player){
            player = _player;
        }

        public void SettingCanvas(GameObject _canvas)
        {
            canvas = _canvas.GetComponent<Canvas>();
        }

    }
}