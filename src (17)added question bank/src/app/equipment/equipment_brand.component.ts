import { Equipment_Brand } from './../_models/equipment_brand';
import { EquipmentService } from './../_services/equipment.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { Course } from '../_models';
import { CourseService, AlertService } from '../_services';

import { Router } from '@angular/router';
import { ModalService } from '../_modal';

@Component({
  templateUrl: 'equipment_brand.component.html',
  styleUrls: ['./ss_equipment.component.css']
})

export class Equipment_BrandComponent implements OnInit {
  equipment_brand: any;

  searchText = '';
  item: any;
  date!: string;
  myValue = 0;

  newEquipment_TypeClicked = false;

  model: any = {};
  model2: any = {};

  model3: Equipment_Brand = {
      EquipmentBrandId: 0,
      EquipmentBrandName: ''
  };

  constructor(
    private equipmentService: EquipmentService,
    private alertService: AlertService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.loadAll();
    this.date = new Date().toISOString().slice(0, 10);
  }

  private loadAll() {
    this.equipmentService.getAllEquipment_Brand()
      .pipe(first())
      .subscribe(
        equipment_brand => {
          this.equipment_brand = equipment_brand;
        },
        error => {
          this.alertService.error('Error, Data was unsuccesfully retrieved');
        }
      );
  }

  addEquipment_Type() {
    if (Object.keys(this.model).length < 1) {
      this.alertService.error("Error, you have an empty feild");
      this.newEquipment_TypeClicked = !this.newEquipment_TypeClicked;
      this.model = {};
    }
    else if ((Object.keys(this.model).length == 1)) {
      this.model3.EquipmentBrandName = this.model.EquipmentBrandName;

      this.equipmentService.createBrand(this.model3)
        .pipe(first())
        .subscribe(
          data => {
            this.alertService.success('Creation was successful', true);
            this.loadAll();
            this.newEquipment_TypeClicked = !this.newEquipment_TypeClicked;
            this.model = {};
          },
          error => {
            this.alertService.error('Error, Creation was unsuccesful');
            this.loadAll();
            this.newEquipment_TypeClicked = !this.newEquipment_TypeClicked;
          });
    }
  }

  deleteEquipment_Type(i: number) {
    this.equipmentService.deleteType(i)
      .pipe(first())
      .subscribe(
        data => {
          this.alertService.success('Deletion was successful', true);
          this.loadAll();
        },
        error => {
          this.alertService.error('Error, Deletion was unsuccesful');
          this.loadAll();
        });
  }

  editEquipment_Type(editEquipment_TypeInfo: number) {
    this.model2.EquipmentBrandName = this.equipment_brand[editEquipment_TypeInfo].equipmentBrandName;
    this.myValue = editEquipment_TypeInfo;
  }

  updateEquipment_Type() {
    let editEquipment_TypeInfo = this.myValue;

    for (let i = 0; i < this.equipment_brand.length; i++) {

      if (i == editEquipment_TypeInfo) {
        this.model3.EquipmentBrandName = this.model2.EquipmentBrandName;

        this.equipmentService.updateBrand(this.equipment_brand[editEquipment_TypeInfo].equipmentBrandId, this.model3)
          .pipe(first())
          .subscribe(
            data => {
              this.alertService.success('Update was successful', true);
              this.loadAll();
              this.model2 = {};
            },
            error => {
              this.alertService.error('Error, Update was unsuccesful');
              this.loadAll();
            });
      }
    }
  }

  addNewCourseBtn() {
    this.newEquipment_TypeClicked = !this.newEquipment_TypeClicked;
  }

}
