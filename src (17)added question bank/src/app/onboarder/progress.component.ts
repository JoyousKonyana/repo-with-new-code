import { ProgressReport } from './../_models/progressreport';
import { OnboarderService } from './../_services/onboarder.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { first } from 'rxjs/operators';

import { User } from '../_models';
import { UserService, AuthenticationService, AlertService } from '../_services';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { ManageCoursesService } from '../_services/manage-courses/manage-courses.service';
import { HttpEventType } from '@angular/common/http';
import { N_AnswerOption } from '../_services/manage-courses/manage-courses.types';

@Component({
  templateUrl: 'progress.component.html',
  styleUrls: ['./ss_onboarder.component.css']
})

export class ProgressComponent implements OnInit {
  progress: any = {};

  courseId: any;
  onboarderId: any;

  model: ProgressReport = {
    onboarderID: 1,
    courseID: 1
  }

  achievements: any[] = [];

  constructor(
    private onboarderService: OnboarderService,
    private _Activatedroute: ActivatedRoute,
    private _router: Router,
    private alertService: AlertService,

    private _manageCoursesService: ManageCoursesService,
    private _ngxSpinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
  ) {
  }

  token: any;

  onboarderid!: Number;
  ngOnInit() {

    this.token = localStorage.getItem('token');

    this._Activatedroute.paramMap.subscribe(params => {
      this.courseId = params.get('id');
    });
    var movies = localStorage.getItem("user");
    movies = JSON.parse(movies);
    this.onboarderid = movies['onboarderid'] as number;
    console.log("onboadre number", this.onboarderid)

    this.getAllAnswerOptionsFromServer();
  }

  private getAllAnswerOptionsFromServer() {
    this._manageCoursesService.getOnborderAchievementsByCourse(this.onboarderid, this.courseId).subscribe(event => {
      if (event.type === HttpEventType.Sent) {
        this._ngxSpinner.show();
      }
      if (event.type === HttpEventType.Response) {
        this.achievements = event.body as any[];
        console.log(this.achievements)
        this._ngxSpinner.hide();
      }
    },
      error => {
        this._ngxSpinner.hide();
        this.openSnackBar("Error:", error.error.message, 3000);
      });
  }

  private openSnackBar(message: string, action: string, _duration: number) {
    this._snackBar.open(message, action, {
      duration: _duration,
      verticalPosition: 'top'
    });
  }

}
