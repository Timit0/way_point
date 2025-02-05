#if TOOLS
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Godot;
using Godot.Collections;

[Tool]
public partial class WP3DRoute : Node
{
    [Export]
    public string RouteName { get; set; }
    [Export]
    public bool NeedToBeReachedInOrder { get; set; } = true;
    [Export]
    public Array<WayPoint3D> WayPoints { get; set; } = new Array<WayPoint3D>();

    [ExportGroup("Debug")]
    [Export]
    public bool Loopable { get; set; } = false;
    [Export]
    protected bool DrawLineInRuntime {get;set;} = false;
    [Export]
    public Color ColorOfLines { get; set; } = new Color(0.9922f, 0.2392f, 0.7098f, 1);
    [Export]
    public bool ShowLabel { get; set; } = true;
    [Export]
    public float LabelPixelSize { get; set; } = 0.06f;


    [Signal]
    public delegate void LapCompletedEventHandler(WP3DRoute wp3dRoute, Node3D node);

    public Godot.Collections.Dictionary<string, Array<WayPoint3D>> LapHistory { get; set; } = new Godot.Collections.Dictionary<string, Array<WayPoint3D>>();

    public override void _Ready()
    {
        WP3DSignals.Instance.BodyEntered += on_body_entered;
        WP3DSignals.Instance.BodyExited += on_body_exited;

        WP3DRouteSignals.Instance.LapCompleted += on_lap_completed;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(Engine.IsEditorHint() || DrawLineInRuntime)
        {
            foreach (Node node in GetChildren())
            {
                if (node is MeshInstance3D wp3d)
                {
                    this.RemoveChild(wp3d);
                }
            }

            DrawLinkedLines(this.WayPoints, this.ColorOfLines);

            if (Engine.IsEditorHint())
            {
                UpdateLabels(WPSingleton.Instance.WP3DRouteAreEditorSelected, this.WayPoints, this.ShowLabel, this.LabelPixelSize);
            }
        }
    }

    private void on_body_entered(WayPoint3D wp3d, Node3D nodeEntered)
    {
        if (WP3DIsInThisWP3DRoute(this.WayPoints, wp3d))
        {
            UpdateLapHistory(this.WayPoints, wp3d, nodeEntered, this.NeedToBeReachedInOrder);
        }
    }

    private void on_body_exited(WayPoint3D wp3d, Node3D nodeExited)
    {
        if (WP3DIsInThisWP3DRoute(this.WayPoints, wp3d))
        {

        }
    }

    private void on_lap_completed(WP3DRoute wp3dRoute, Node3D node)
    {
        if (wp3dRoute != this)
        {
            return;
        }
        this.Emit_LapCompleted_Signal(wp3dRoute, node);
        this.LapHistory[node.ToString()].Clear();
    }

    public void DrawLinkedLines(Array<WayPoint3D> wayPoints, Color colorOfLine)
    {
        if (VerifyIfWayPointsAreOkay(wayPoints))
        {
            if (wayPoints.Count > 1)
            {
                for (int i = 0; i < wayPoints.Count - 1; i++)
                {
                    MeshInstance3D meshInstance = new MeshInstance3D();
                    DrawLine(wayPoints[i].GlobalPosition, wayPoints[i + 1].GlobalPosition, colorOfLine, meshInstance);
                }
            }

            if (wayPoints.Count > 1)
            {
                if (Loopable)
                {
                    MeshInstance3D meshInstance = new MeshInstance3D();
                    DrawLine(wayPoints[0].GlobalPosition, WayPoints[wayPoints.Count - 1].GlobalPosition, colorOfLine, meshInstance);
                }
            }
        }
    }

    public void Emit_LapCompleted_Signal(WP3DRoute wp3dRoute, Node3D node)
    {
        this.EmitSignal(SignalName.LapCompleted, wp3dRoute, node);
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color, MeshInstance3D meshInstance)
    {
        ImmediateMesh mesh = new ImmediateMesh();
        StandardMaterial3D standardMaterial = new StandardMaterial3D
        {
            VertexColorUseAsAlbedo = true,
        };

        mesh.SurfaceBegin(Mesh.PrimitiveType.LineStrip);

        mesh.SurfaceSetColor(color);

        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);

        mesh.SurfaceEnd();

        this.AddChild(meshInstance);
        meshInstance.Mesh = mesh;
        meshInstance.MaterialOverride = standardMaterial;
    }

    protected void UpdateLapHistory(Array<WayPoint3D> wayPoints, WayPoint3D wp3d, Node3D nodeEntered, bool inOrderOnly)
    {
        if (inOrderOnly)
        {
            //if not contains nodeEntered
            if (!LapHistory.ContainsKey(nodeEntered.ToString()))
            {
                //if it's the first wp3d
                if (GetIndexOfAWayPointInArray(wayPoints, wp3d) == 0)
                {
                    LapHistory.Add(nodeEntered.ToString(), new Array<WayPoint3D>() { wp3d });
                }
            }
            else if (LapHistory.ContainsKey(nodeEntered.ToString()))
            {
                if (this.LapHistory[nodeEntered.ToString()].Count == GetIndexOfAWayPointInArray(wayPoints, wp3d))
                {
                    LapHistory[nodeEntered.ToString()].Add(wp3d);
                }
            }

            if (LapHistory.ContainsKey(nodeEntered.ToString()) && LapHistory[nodeEntered.ToString()].Count == wayPoints.Count)
            {
                WP3DRouteSignals.Instance.Emit_LapCompleted_Signal(this, nodeEntered);
            }
        }
        else
        {
            //if not contains nodeEntered
            if (!LapHistory.ContainsKey(nodeEntered.ToString()))
            {
                LapHistory.Add(nodeEntered.ToString(), new Array<WayPoint3D>() { wp3d });

            }
            else if (LapHistory.ContainsKey(nodeEntered.ToString()))
            {
                if (!this.LapHistory[nodeEntered.ToString()].Contains(wp3d))
                {
                    LapHistory[nodeEntered.ToString()].Add(wp3d);
                }

                if (LapHistory[nodeEntered.ToString()].Count == this.WayPoints.Count)
                {
                    WP3DRouteSignals.Instance.Emit_LapCompleted_Signal(this, nodeEntered);
                }
            }
        }
    }

    protected bool WP3DIsInThisWP3DRoute(Array<WayPoint3D> wayPoints, WayPoint3D wayPoint)
    {
        if (wayPoints.Contains(wayPoint))
        {
            return true;
        }
        return false;
    }

    public int GetIndexOfAWayPointInArray(Array<WayPoint3D> wayPoints, WayPoint3D wayPoint)
    {
        int iOut = 0;
        for (int i = 0; i < wayPoints.Count; i++)
        {
            if (wayPoint == wayPoints[i])
            {
                iOut = i;
            }
        }
        return iOut;
    }

    public void UpdateLabels(List<WP3DRoute> routesSelected, Array<WayPoint3D> wayPoints, bool showLabel, float labelPixelSize)
    {
        if (!showLabel)
        {
            return;
        }

        foreach (WP3DRoute wp3dRoute in routesSelected)
        {
            if (wp3dRoute == this)
            {
                foreach (WayPoint3D wp3d in wayPoints)
                {
                    if (!VerifyIfWayPointsAreOkay(wayPoints) || wp3d is null)
                    {
                        return;
                    }

                    string label = "";
                    for (int i = 0; i < wayPoints.Count; i++)
                    {
                        if (wp3d == wayPoints[i])
                        {
                            label = (i + 1).ToString();
                        }
                    }
                    wp3d.Label3DNumber.Text = label;
                    wp3d.Label3DNumber.PixelSize = labelPixelSize;
                }
            }
        }
    }

    protected bool VerifyIfWayPointsAreOkay(Array<WayPoint3D> wayPoints)
    {
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            if (wayPoints[i] is null || WayPoints[i + 1] is null || !WayPoints[i].IsInsideTree() || !WayPoints[i + 1].IsInsideTree())
            {
                return false;
            }
        }
        return true;
    }
}
#endif