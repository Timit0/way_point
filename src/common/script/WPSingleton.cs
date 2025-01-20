using System.Collections.Generic;

public partial class WPSingleton : Singleton<WPSingleton>
{
    private WPSingleton(){}

    public List<WP3DRoute> WP3DRouteAreEditorSelected {get;set;} = new List<WP3DRoute>();
}