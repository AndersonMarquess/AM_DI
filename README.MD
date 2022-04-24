
<h2>How to use?</h2>
<p>1 - Just add the desired attribute on the field that will be filled.</p>
<p>2 - In the transform of the gameobject, just click on "Find"</p>

![Transform component image with custom editor showing the Find button](/AM_DI.jpg)

<p>3 - Done!</p>

<h2>Usage example in code</h2>
<hr>

```cs
    [SerializeField, FindInScene]
    private Light _sceneMainLight = null;
    
    [SerializeField, FindInScene(ActiveOnly = true)]
    private SphereCollider _activeSphereInScene = null;

    [SerializeField, FindInComponent]
    private NavMeshAgent _myNavMeshAgent = null;

    [SerializeField, FindInChild]
    private Renderer _childRenderer = null;
    
    [SerializeField, FindInChild(Path = "Plane/InnerChild")]
    private Renderer _specifChildRenderer = null;
```
<hr>
<h2>Currently supported operations</h2>

- Find In Component
- Find In Child
    - Active
    - With Path
- Find In Scene
    - Active

<h2>Why was it created?</h2>
<p>Drag and drop can be time consuming when done over and over again, having a button to fill dependencies saved me a lot of time.</p>

<h2>License</h2>

- Yes, can be used commercially.
- No, you don't have to credit me, but that would be nice.
- MIT License.

<h2>Support</h2>

- No thank you. I usually search on google.

