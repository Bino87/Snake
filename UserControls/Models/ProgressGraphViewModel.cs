﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UserControls.Constants;
using UserControls.Core.Base;
using UserControls.Objects.NeuralNetDisplay;

namespace UserControls.Models
{
    public interface IProgressGraphValueRegister
    {
        void Register(double value);
    }

    public class ProgressGraphViewModel : Observable, IProgressGraphValueRegister
    {
        private readonly List<double> _results;
        private double _min = double.MaxValue;
        private double _max = double.MinValue;

        public ObservableCollection<ProgressGraphLine> DataSource { get; set; }

        public ProgressGraphViewModel()
        {
            DataSource = new ObservableCollection<ProgressGraphLine>();
            _results = new List<double>();
        }

        public void Register(double value)
        {
            _results.Add(value);

            if (_max < value) _max = value;
            if (_min > value) _min = value;

            if (_results.Count > 1)
            {
                DataSource.Clear(); 
                for (int i = 1; i < _results.Count; i++)
                {
                    double x1 = (i - 1d) / _results.Count * Cons.cProgressGraphWidth;
                    double x2 = ((double)i) / _results.Count * Cons.cProgressGraphWidth;
                    double y1 = Cons.cProgressGraphHeight - InverseLerp(_results[i - 1]) * Cons.cProgressGraphHeight;
                    double y2 = Cons.cProgressGraphHeight - InverseLerp(_results[i]) * Cons.cProgressGraphHeight;

                    ProgressGraphLine pgl = new(x1, x2, y1, y2);
                    DataSource.Add(pgl);
                }
            }
        }

        double InverseLerp(double value)
        {
            if (value <= _min)
                return 0;
            if (value >= _max)
                return 1;
            return (value - _min) / (_max - _min);
        }
    }
}