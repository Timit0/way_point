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
    public bool Loopable { get; set; } = false;
    [Export]
    public bool NeedToBeReachedInOrder {get;set;} = true;
    [Export]
    public Array<WayPoint3D> WayPoints { get; set; } = new Array<WayPoint3D>();

    [ExportGroup("Debug")]
    [Export]
    public Color ColorOfLines { get; set; } = new Color(0.9922f, 0.2392f, 0.7098f, 1);
    [Export]
    public bool ShowLabel {get;set;} = true;
    [Export]
    public float LabelPixelSize {get;set;} = 0.06f;

    public override void _Ready()
    {
        WP3DSignals.Instance.BodyEntered += on_body_entered;
        WP3DSignals.Instance.BodyExited += on_body_exited;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        foreach (Node node in GetChildren())
        {
            if (node is MeshInstance3D wp3d)
            {
                this.RemoveChild(wp3d);
            }
        }

        DrawLinkedLines(this.WayPoints, this.ColorOfLines);

        if(Engine.IsEditorHint())
        {
            UpdateLabels(WPSingleton.Instance.WP3DRouteAreEditorSelected, this.WayPoints, this.ShowLabel, this.LabelPixelSize);
        }
    }

    private void on_body_entered(WayPoint3D wp3d, Node3D nodeEntered)
    {
        GD.Print("ENTERED");
    }

    private void on_body_exited(WayPoint3D wp3d, Node3D nodeExited)
    {
        // GD.Print("EXITED");
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

    public void DrawLine(Vector3 start, Vector3 end, Color color, MeshInstance3D meshInstance)
    {
        ImmediateMesh mesh = new ImmediateMesh();
        StandardMaterial3D standardMaterial = new StandardMaterial3D
        {
            VertexColorUseAsAlbedo = true,
            // SubsurfScatterStrength = 12
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

    public void UpdateLabels(List<WP3DRoute> routesSelected, Array<WayPoint3D> wayPoints, bool showLabel, float labelPixelSize)
    {
        if(!showLabel)
        {
            return;
        }

        foreach (WP3DRoute wp3dRoute in routesSelected)
        {
            if(wp3dRoute == this)
            {
                foreach (WayPoint3D wp3d in wayPoints)
                {
                    if(!VerifyIfWayPointsAreOkay(wayPoints) || wp3d is null)
                    {
                        return;
                    }

                    string label = "";
                    for (int i = 0; i < wayPoints.Count; i++)
                    {
                        if(wp3d == wayPoints[i])
                        {
                            label = (i+1).ToString();
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