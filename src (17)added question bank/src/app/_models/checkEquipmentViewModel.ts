import { Datetime } from "ng-modules";

export class checkEquipmentViewModel {
     EquipmentId!:number; 
       OnboarderId:number;
        //public DateTime EquipmentCheckOutDate { get; set; }

        //public string EquipmentCheckOutCondition { get; set; }

         EquipmentCheckInDate:Datetime;
         EquipmentCheckInCondition:Datetime 
}