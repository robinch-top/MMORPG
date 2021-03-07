using System;
using ETModel;
using UnityEngine;

namespace MMOGame
{
    [UIFactory(UIType.UISelection)]
    public class UISelectionFactory:IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject parent)
        {
	        try
	        {
                UI ui = UIResources.Get(type);

				ui.AddComponent<UISelectionComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

		public void Remove(string type)
        {
            Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
        }
    }
}