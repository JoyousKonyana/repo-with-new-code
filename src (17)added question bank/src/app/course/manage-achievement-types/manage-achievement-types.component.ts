import { HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { first } from 'rxjs/operators';
import { Course } from 'src/app/_models';
import { CourseService, AlertService } from 'src/app/_services';
import { ManageCoursesService } from 'src/app/_services/manage-courses/manage-courses.service';
import { N_AchievementTye, N_questionBankWithQuestions } from 'src/app/_services/manage-courses/manage-courses.types';

@Component({
  selector: 'app-manage-achievement-types',
  templateUrl: './manage-achievement-types.component.html',
  styleUrls: ['./manage-achievement-types.component.css']
})
export class ManageAchievementTypesComponent implements OnInit {
  types: N_AchievementTye[] = [];
  addTypeForm: FormGroup;
  addNewCliecked = false;

  constructor(
    private alertService: AlertService,
    private _activatedRoute: ActivatedRoute,
    private _manageCoursesService: ManageCoursesService,
    private _ngxSpinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
    private _router: Router,
    private _dialog: MatDialog,
    private _formBuilder: FormBuilder
  ) {
    this.buildAddForm();
  }



  ngOnInit() {
    this.getTypesFromServer();
  }

  private getTypesFromServer() {
    this._manageCoursesService.getAchivementTypes().subscribe(event => {
      if (event.type === HttpEventType.Sent) {
        this._ngxSpinner.show();
      }
      if (event.type === HttpEventType.Response) {
        this.types = event.body as N_AchievementTye[];
        this._ngxSpinner.hide();
      }
    },
      error => {
        this._ngxSpinner.hide();
        this.openSnackBar("Error:", error.error.message, 3000);
      });
  }


  onShowAddNewUser() {
    this.addNewCliecked = !this.addNewCliecked;
  }

  onAddSubmit() {

    if (this.addTypeForm.valid) {
      console.log(this.addTypeForm.value);
      this._manageCoursesService.addAchievementType(this.addTypeForm.value)
        .subscribe(event => {
          if (event.type === HttpEventType.Sent) {
            this._ngxSpinner.show();
          }
          if (event.type === HttpEventType.Response) {
            this.getTypesFromServer();
            this._ngxSpinner.hide();
            this.addNewCliecked = false;
            this.openSnackBar("Add Achievement Type", "Success!", 3000);
          }
        },
          error => {
            this._ngxSpinner.hide();
            this.openSnackBar("Error", error.error.message, 3000);
          });
    }
  }

  private buildAddForm() {
    this.addTypeForm = this._formBuilder.group({
      Name: ['', [Validators.required]],
      MinMark: ['', [Validators.required]],
      MaxMark: ['', [Validators.required]]
    });
  }

  get Name() { return this.addTypeForm.get('Name') }
  get MinMark() { return this.addTypeForm.get('MinMark') }
  get MaxMark() { return this.addTypeForm.get('MaxMark') }

  private openSnackBar(message: string, action: string, _duration: number) {
    this._snackBar.open(message, action, {
      duration: _duration,
      verticalPosition: 'top'
    });
  }



}
