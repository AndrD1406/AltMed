﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.DataAccess.Models;

public interface IKeyedEntity<TKey> : IKeyedEntity
{
    TKey Id { get; set; }
}

public interface IKeyedEntity
{
}
