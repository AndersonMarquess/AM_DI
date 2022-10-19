<h2>How to install?</h2>

Add as custom git package with package manager:

1. Open package manager
2. Click on the plus (➕) button
3. Choose "Add package from git URL"
4. Past the url: 
```json
https://github.com/AndersonMarquess/AM_DI.git?path=/Packages/DependencyInitializer
```

or

Set direct reference in manifest.json:
```json
"com.andersonmarques.dependencyinitializer": "https://github.com/AndersonMarquess/AM_DI.git?path=/Packages/DependencyInitializer"
```

<h2>How to use?</h2>
<p>1 - Just add the desired attribute on the field that will be filled.</p>
<p>2 - In the transform of the gameobject, just click on "Find"</p>

![Transform component image with custom editor showing the Find button](https://raw.githubusercontent.com/AndersonMarquess/AM_DI/main/AM_DI.jpg)

<p>3 - Done!</p>

<h2>Usage example in code</h2>
<hr>

```cs
    [SerializeField, FindInScene]
    private Light _sceneMainLight = null;
    
    [SerializeField, FindInScene(ActiveOnly = true)]
    private SphereCollider _activeSphereInScene = null;
        
    [SerializeField, FindInParent]
    private MockComponent _parentComponent = null;

    [SerializeField, FindInComponent]
    private NavMeshAgent _myNavMeshAgent = null;

    [SerializeField, FindInChild]
    private Renderer _childRenderer = null;
    
    [SerializeField, FindInChild(Path = "Plane/InnerChild")]
    private Renderer _specifChildRenderer = null;
```
<hr>
<h2>Currently supported operations</h2>

- Find In Scene
    - Active
- Find In Parent
- Find In Component
- Find In Child
    - Active
    - With Path

<h2>Why was it created?</h2>
<p>Drag and drop can be time consuming when done over and over again, having a button to fill dependencies saved me a lot of time.</p>

<h2>License</h2>

- Yes, can be used commercially.
- No, you don't have to credit me, but that would be nice.
- MIT License.
