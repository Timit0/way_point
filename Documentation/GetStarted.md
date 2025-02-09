# Get Started
## Installation
Go to the AssetLib and download Way Point then enable the plugin in project settings.

## Nodes explaination
<details>
  <summary>WayPoint3D</summary>
  
  ``WayPoint3D`` is a node who inherit from ``Node3D`` and who contain an ``Area3D``.

  This node act like a trigger point, like an ``Arrea3D`` you can choose ``Mask`` and ``Layer`` collision. You can also change the size of the ``BoxShape``.

  A node can be in multiple "Route" at same time.
</details>

<details>
  <summary>WP3DRoute</summary>
  
  ``WP3DRoute`` is node who inherit from ``Node``.
  This Node is the "Route".

  You need to atached at least 2 ``WayPoint3D``.
  You need to specify a ``RouteName``.
  This node has some debug settings (in editor and in runtime).
</details>

## Create your first route
It's very simple.

Add multiple ``WayPoint3D`` and a ``WP3DRoute``, in the ``WP3DRoute`` set a ``RouteName`` and add the way points.

Done.

## WPR3DRoute signals
When a lap is completed a signal is sending to <b>ALL</b> the `WP3DRoute`. The signal send you 2 object, the `WP3DRoute` who your object had made a lap and the object in question a `Node3D`

You can connect in editor to ``LapCompleted`` signal.
```
private void on_lap_completed(WP3DRoute wp3dRoute, Node3D node)
{
    //You probably need to test if the lap is this route or not

    //Your Code Here
}
```

You can connect in the code directely

```
WP3DRoute MyWP3DRoute;

public override void _Ready()
{
    MyWP3DRoute.LapCompleted += on_lap_completed;
    base._Ready();
}

private void on_lap_completed(WP3DRoute wp3dRoute, Node3D node)
{
    //Verify if the lap completed is this route
    if(wp3dRoute != MyWP3DRoute)
    {
        return;
    }

    //Your Code Here
}
```