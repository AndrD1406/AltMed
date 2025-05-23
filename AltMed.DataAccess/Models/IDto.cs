﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.DataAccess.Models;

public interface IDto<TEntity, TKey>
    where TEntity : IKeyedEntity<TKey>
{
    TKey Id { get; set; }
}
