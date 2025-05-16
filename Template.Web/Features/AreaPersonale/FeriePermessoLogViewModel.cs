using System;
using System.Collections.Generic;

namespace Template.Web.Features.AreaPersonale
{
    public class FeriePermessoLogViewModel
{
    // La data del permesso o ferie in formato stringa (ad esempio, "15/05/2025")
    public string Data { get; set; }

    // Tipo di permesso: può essere "Ferie" o "Permesso"
    public string Tipo { get; set; }
}
}
