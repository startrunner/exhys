
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Exhys.WebContestHost.DataModels
{

using System;
    using System.Collections.Generic;
    
public partial class Participation
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Participation()
    {

        this.Submissions = new HashSet<ProblemSolution>();

    }


    public int Id { get; set; }



    public virtual UserAccount User { get; set; }

    public virtual Competition Competition { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ProblemSolution> Submissions { get; set; }

}

}
