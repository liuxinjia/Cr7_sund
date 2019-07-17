public class Player {
    string name;
    public string Name {
        get {
            return name;
        }
        set {
            name = value;
        }
    }

    public Player (string _name) {
        name = _name;
    }
}

public class Program {
    public static void Main (string[] args) {
        Player kobe = new Player ("Kobe");
        Player james = new Player ("James");

        swap (kobe, james);

        Console.WriteLine ("Kobe name = " + kobe.Name);
        Console.WriteLine ("James name = " + james.Name);

        Idiot (kobe);
        Console.WriteLine ("Kobe name = " + kobe.Name);
    }

    public static void swap (Object o1, Object o2) {
        var temp = o1;
        o1 = o2;
        o2 = o1;
    }

    public static void Idiot (Player player) {
        player.Name = "James";
        player = new Player ("Ronaldo");
        player.Name = "Kobe";

    }
}