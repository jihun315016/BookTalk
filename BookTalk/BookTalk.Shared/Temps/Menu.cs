using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Temps;

public class Menu
{
    public int ID { get; set; }

    public int Level { get; set; }

    public string ControllerName { get; set; }

    public string ActionName { get; set; }

    public string MenuName { get; set; }

    public int ParentMenuID { get; set; }
}
