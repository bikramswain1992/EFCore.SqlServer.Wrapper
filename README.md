EFCore.SqlServer.Wrapper
=========================
Provides easy configuration of EFCore SQL server with dependancy injection. Give access to basic functionalities with inbuilt error handeling and formatted result.

Usage
---------
Library can be used to create single or multiple **DbContext** depending on the need. As a example we will consider to database entities **Country** and **State**. Below is the structure for these entities.

  public class Country : VersionedEntityBase  
  {  
      public string Name {get; set;}  
      public string IsdCode {get; set;}  
  }  
    
  public class State : EntityBase  
  {  
      public string Name {get; set;}  
      public int Population {get; set;}  
      public Guid CountryId {get; set;}  
  }  

