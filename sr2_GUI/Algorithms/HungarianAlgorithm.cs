using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    public sealed class HungarianAlgorithm     
    {
        private readonly int[,] _costMatrix;   //матрица производительностей
        private int _inf;
        private int _n;                        //количество работ-работников
        private int[] _V;                     //"поблажки" для работнков
        private int[] _U;                     //
        private bool[] _s;
        private bool[] _t;
        private int[] _matchX;                 //vertex matched with x
        private int[] _matchY;                 //vertex matched with y
        private int _maxMatch;
        private int[] _slack;
        private int[] _slackx;
        private int[] _prev;                    //запоминание путей

  
        public HungarianAlgorithm(int[,] costMatrix)
        {
            _costMatrix = costMatrix;
        }

        public int[] Run()
        {
            _n = _costMatrix.GetLength(0);

            _V = new int[_n];
            _U = new int[_n];
            _s = new bool[_n];
            _t = new bool[_n];
            _matchX = new int[_n];
            _matchY = new int[_n];
            _slack = new int[_n];
            _slackx = new int[_n];
            _prev = new int[_n];
            _inf = int.MaxValue;

            //заполнение _matchX _matchY
            InitMatches();

            //если матрица не ровная возвращаем 0
            if (_n != _costMatrix.GetLength(1))
                return null;

            InitLbls();

            _maxMatch = 0;

            InitialMatching();

            var q = new Queue<int>();

            while (_maxMatch != _n)
            {
                q.Clear();
                for (var i = 0; i < _n; i++)
                {
                    _s[i] = false;
                    _t[i] = false;
                }


                var root = 0;
                int x;
                var y = 0;

                //find root of the tree
                for (x = 0; x < _n; x++)
                {
                    if (_matchX[x] != -1) continue; //i++
                    q.Enqueue(x);
                    root = x;
                    _prev[x] = -2;

                    _s[x] = true;
                    break;
                }

                //init slack
                for (var i = 0; i < _n; i++)
                {
                    _slack[i] = _costMatrix[root, i] - _V[root] - _U[i];
                    _slackx[i] = root;
                }

                //finding augmenting path
                while (true)
                {
                    while (q.Count != 0)
                    {
                        x = q.Dequeue();
                        var lxx = _V[x];
                        for (y = 0; y < _n; y++)
                        {
                            if (_costMatrix[x, y] != lxx + _U[y] || _t[y]) continue;
                            if (_matchY[y] == -1) break; //augmenting path found!
                            _t[y] = true;
                            q.Enqueue(_matchY[y]);

                            AddToTree(_matchY[y], x);
                        }
                        if (y < _n) break; //augmenting path found!
                    }
                    if (y < _n) break; //augmenting path found!
                    UpdateLabels(); //augmenting path not found, update labels

                    for (y = 0; y < _n; y++)
                    {

                        if (_t[y] || _slack[y] != 0) continue;
                        if (_matchY[y] == -1) //found exposed vertex-augmenting path exists
                        {
                            x = _slackx[y];
                            break;
                        }
                        _t[y] = true;
                        if (_s[_matchY[y]]) continue;
                        q.Enqueue(_matchY[y]);
                        AddToTree(_matchY[y], _slackx[y]);
                    }
                    if (y < _n) break;
                }

                _maxMatch++;

                int ty;
                for (int cx = x, cy = y; cx != -2; cx = _prev[cx], cy = ty)
                {
                    ty = _matchX[cx];
                    _matchY[cy] = cx;
                    _matchX[cx] = cy;
                }
            }

            return _matchX;
        }

        private void InitMatches()
        {
            for (var i = 0; i < _n; i++)
            {
                _matchX[i] = -1;
                _matchY[i] = -1;
            }
        }

        private void InitLbls()
        {
            for (var i = 0; i < _n; i++)
            {
                var minRow = _costMatrix[i, 0];
                for (var j = 0; j < _n; j++)
                {
                    if (_costMatrix[i, j] < minRow) minRow = _costMatrix[i, j];
                    if (minRow == 0) break;
                }
                _V[i] = minRow;
            }
            for (var j = 0; j < _n; j++)
            {
                var minColumn = _costMatrix[0, j] - _V[0];
                for (var i = 0; i < _n; i++)
                {
                    if (_costMatrix[i, j] - _U[i] < minColumn) minColumn = _costMatrix[i, j] - _V[i];
                    if (minColumn == 0) break;
                }
                _U[j] = minColumn;
            }
        }

        private void UpdateLabels()
        {
            var delta = _inf;
            for (var i = 0; i < _n; i++)
                if (!_t[i])
                    if (delta > _slack[i])
                        delta = _slack[i];
            for (var i = 0; i < _n; i++)
            {
                if (_s[i])
                    _V[i] = _V[i] + delta;
                if (_t[i])
                    _U[i] = _U[i] - delta;
                else _slack[i] = _slack[i] - delta;
            }
        }

        private void AddToTree(int x, int prevx)
        {
            _s[x] = true; //adding x to S
            _prev[x] = prevx;

            var lxx = _V[x];
            //updateing slack
            for (var y = 0; y < _n; y++)
            {
                if (_costMatrix[x, y] - lxx - _U[y] >= _slack[y]) continue;
                _slack[y] = _costMatrix[x, y] - lxx - _U[y];
                _slackx[y] = x;
            }
        }

        private void InitialMatching()
        {
            for (var x = 0; x < _n; x++)
            {
                for (var y = 0; y < _n; y++)
                {
                    if (_costMatrix[x, y] != _V[x] + _U[y] || _matchY[y] != -1) continue;
                    _matchX[x] = y;
                    _matchY[y] = x;
                    _maxMatch++;
                    break;
                }
            }
        }
    }
}
