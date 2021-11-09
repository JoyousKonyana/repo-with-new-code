import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { checkEquipmentViewModel } from 'src/app/_models';
import { Equipment_QueryService } from 'src/app/_services';

@Component({
  selector: 'app-checkequip',
  templateUrl: './checkequip.component.html',
  styleUrls: ['./checkequip.component.css']
})
export class CheckequipComponent implements OnInit {

  constructor(private form: FormBuilder,  private yService: Equipment_QueryService,@Inject(MAT_DIALOG_DATA) public data: {
    EquipmentId: Number,
    OnboarderId:  Number ,
    EquipmentCheckInDate: Date,
    EquipmentCheckInCondition: string,
    
   },  public dialogRef: MatDialogRef<CheckequipComponent>) { }
otherdata:any;
  ngOnInit(): void {
this.otherdata = this.data;
  }
  
  CheckequipmentForm = this.form.group({
    checkoutCondition: new FormControl('', Validators.required),
  
  
  })
formdata!:string;
  submit(){
this.formdata =this.CheckequipmentForm.get('checkoutCondition')?.value;

const  data2:checkEquipmentViewModel = {
  EquipmentId: this.otherdata.EquipmentId,
  OnboarderId:  this.otherdata.OnboarderId ,
  EquipmentCheckInDate: new Date,
  EquipmentCheckInCondition: this.otherdata.equipmentCheckInCondition
};


  

  this.yService.checkEquipment(data2)
  // .pipe(first())
  .subscribe(res => {
    console.log('Res: ', res);
    localStorage.setItem('my_equipment', JSON.stringify(data2));
 alert("hello");
  });

  this.dialogRef.close();
  }

 
}
