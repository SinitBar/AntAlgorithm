using System;
using System.Collections.Generic;
using System.Text;

namespace AntAlgorithm
{
    class EdgeEnd
    { // хранит информацию о направленном ребере и о его конечной вершине
        public string Name { get; } // имя конечной вершины ребра
        public double Feromon { get; set; } // количество феромона, оставленного на ребре на данный момент
        public double Distance { get; } // длина пути из начальной вершины ребра в конечную

        protected EdgeEnd() { }
        public EdgeEnd(string name, double distance)
        {
            Name = name;
            Distance = distance;
            Feromon = 0.1; // некоторое начальное значение, чтобы первое найденное решение не повторялось все время
        }

    }
}
