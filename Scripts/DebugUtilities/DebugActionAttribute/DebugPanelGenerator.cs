using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DebugUtilities
{
    public class DebugPanelGenerator : MonoBehaviour
    {
        [SerializeField] private DebugPanelActionProvider actionProvider;
        [SerializeField] private DebugPanelActionInvoker actionPrefab;
        [SerializeField] private DebugPanelPropertySetter propertyPrefab;
        [SerializeField] private DebugPanelPage pagePrefab;
        [SerializeField] private DebugPanelPage content;
        [SerializeField] private Button enablePanelButton;

        private readonly List<GameObject> _items = new List<GameObject>();
        private List<DebugPanelPage> _pages = new List<DebugPanelPage>();

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        private void Clear()
        {
            foreach (var item in _items)
            {
                Destroy(item);
            }

            _items.Clear();


            _pages.Remove(content); // do not destroy root page
            foreach (var item in _pages)
            {
                Destroy(item.gameObject);
            }

            _pages.Clear();
        }

        private void Start()
        {
            Clear();
            _pages.Add(content);
            content.Initialize(() => { gameObject.SetActive(false); });

            foreach (var method in actionProvider.Methods)
            {
                var item = Instantiate(actionPrefab, content.transform);
                item.Initialize(method);
                _items.Add(item.gameObject);
                AddToPage(method.ActionPath, item.gameObject);
            }

            foreach (var property in actionProvider.Properties)
            {
                var item = Instantiate(propertyPrefab, content.transform);
                item.Initialize(property);
                _items.Add(item.gameObject);
                AddToPage(property.ActionPath, item.gameObject);
            }
        }

        private void AddToPage(string path, GameObject item)
        {
            var parts = path.Split('/');
            if (parts.Length == 0)
            {
                Debug.LogWarning("Invalid path in DebugPanelGenerator.");
                return;
            }

            DebugPanelPage page = content;
            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (content.Pages.TryGetValue(parts[i], out var child))
                {
                    page = child;
                }
                else
                {
                    var childPage = Instantiate(pagePrefab, transform);
                    childPage.SetActive(false);

                    // Initialize back button 
                    var thisPage = page;
                    childPage.Initialize(() =>
                    {
                        foreach (var x in _pages) x.SetActive(false);
                        thisPage.SetActive(true);
                    });

                    // Create button for enabling this page
                    var button = Instantiate(enablePanelButton, page.transform);
                    button.GetComponentInChildren<Text>().text = parts[i];
                    button.onClick.AddListener(() =>
                    {
                        foreach (var x in _pages) x.SetActive(false);
                        childPage.SetActive(true);
                    });

                    childPage.name = parts[i];
                    content.Pages.Add(parts[i], childPage);
                    page = childPage;
                    _pages.Add(page);
                }
            }

            item.transform.SetParent(page.transform);
        }
    }
}