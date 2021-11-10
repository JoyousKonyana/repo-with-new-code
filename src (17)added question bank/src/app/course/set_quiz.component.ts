import { Quiz } from './../_models/quiz';
import { QuizService } from './../_services/quiz.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { first } from 'rxjs/operators';
import { AlertService } from '../_services';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder, ValidationErrors } from '@angular/forms';
import { ManageCoursesService } from '../_services/manage-courses/manage-courses.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpEventType } from '@angular/common/http';
import { N_questionBank, N_questionBankWithQuestions, N_questionInBank, N_Quiz } from '../_services/manage-courses/manage-courses.types';
import { ThrowStmt } from '@angular/compiler';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatListOption } from '@angular/material/list';

@Component({
  templateUrl: 'set_quiz.component.html',
  styleUrls: ['./ss_course.component.css']
})

export class Set_QuizComponent implements OnInit {
  minDate: Date;
  myValue = 0;
  newQuizClicked = false;
  updateQuizClicked = false;

  model: Quiz = {
    QuizId: 0,
    LessonOutcomeId: 0,
    QuizDescription: '',
    QuizMarkRequirement: '',
    QuizDueDate: '',
    QuizCompletionDate: '',
    NumberOfQuestions: 5
  }

  lesssonOutcomeId: any;
  quizzes: N_Quiz[] = [];
  addQuizForm: FormGroup;

  questionBanks: N_questionBankWithQuestions[] = [];
  questionsInbank: N_questionInBank[] = [];
  bankQuestionLength = 0;

  selectedQuestions;
  @ViewChild('selectedQuestionsFromBank') selectedQuestionsFromBank: any
  constructor(
    private alertService: AlertService,
    private _activatedRoute: ActivatedRoute,
    private _manageCoursesService: ManageCoursesService,
    private _ngxSpinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
    private _router: Router,
    private _formBuilder: FormBuilder,
  ) {
    this._activatedRoute.paramMap.subscribe(params => {
      this.lesssonOutcomeId = params.get('id');
    });
  }

  ngOnInit() {
    this.getOutcomeQuizzesFromServer();
    this.getQuestionBanksWithQuestionsByOutcomeFromServer();
    this.minDate = new Date()
  }

  editQuiz(editQuizInfo: number) {
    // this.updateQuizClicked = !this.updateQuizClicked;

    // this.model.QuizDescription = this.quiz[editQuizInfo].lessonDescription;
    // this.model.QuizMarkRequirement = this.quiz[editQuizInfo].lessonName;
    // this.model.QuizDueDate = this.quiz[editQuizInfo].quizDueDate;
    // this.model.NumberOfQuestions = this.quiz[editQuizInfo].numberOfQuestions;

    // this.myValue = editQuizInfo;
  }

  updateQuiz() {
    // let editQuizInfo = this.myValue;

    // this.model.LessonOutcomeId = this.id;

    // for (let i = 0; i < this.quiz.length; i++) {
    //   if (i == editQuizInfo) {
    //     this.quizSerivce.updateQuiz(this.quiz[editQuizInfo].quizId, this.model)
    //       .pipe(first())
    //       .subscribe(data => {
    //         this.alertService.success('Quiz was Successfully Updated');
    //         this.loadAll();
    //       }, error => {
    //         this.alertService.success('Unsuccessful Update');
    //         this.loadAll();
    //       });
    //   }
    // }
  }

  addNewQuizBtn() {
    this.newQuizClicked = !this.newQuizClicked;
  }

  closeUpdate() {
    this.updateQuizClicked = !this.updateQuizClicked;
  }


  private getOutcomeQuizzesFromServer() {
    this._manageCoursesService.getQuzzesByLessonOutcomeId(this.lesssonOutcomeId).subscribe(event => {
      if (event.type === HttpEventType.Sent) {
        this._ngxSpinner.show();
      }
      if (event.type === HttpEventType.Response) {
        this.quizzes = event.body as N_Quiz[];
        console.log(this.quizzes[0])
        this._ngxSpinner.hide();
      }
    },
      error => {
        this._ngxSpinner.hide();
        this.alertService.error('Error: Could not return quizzes');
      });
  }

  private getQuestionBanksWithQuestionsByOutcomeFromServer() {
    this._manageCoursesService.getQuestionBanksWithQuestionsByLessonOutcomeId(this.lesssonOutcomeId).subscribe(event => {
      if (event.type === HttpEventType.Sent) {
        this._ngxSpinner.show();
      }
      if (event.type === HttpEventType.Response) {
        this.questionBanks = event.body as N_questionBankWithQuestions[];
        if(this.questionBanks.length ==0){
          this.alertService.error('Error:Create a question bank first');
          return
        }

        this.buildAddQuizForm();
        this._ngxSpinner.hide();
      }
    },
      error => {
        this._ngxSpinner.hide();
        this.alertService.error('Error: Could not return question banks');
      });
  }

  onGetBankQuestionsCount(bank: N_questionBankWithQuestions) {
    this.questionsInbank = bank.questions;
  }

  onGroupsChange(options: MatListOption[]) {
    this.selectedQuestionsFromBank = options.map(o => o.value);
  }

  onSubmitQuiz() {
    this.addQuizForm.controls['Questions'].setValue(this.selectedQuestionsFromBank);
    this.addQuizForm.controls['OutcomeId'].setValue(this.lesssonOutcomeId);

    console.log(this.addQuizForm.value);
    var questionAsArray = this.Questions.value as [];

    if (this.addQuizForm.valid) {

      if (!Array.isArray(this.selectedQuestionsFromBank)) {
        this.openSnackBar("Error!", "Select atleast one quesiton from bank", 3000);
        return;
      }
      if (questionAsArray.length == 0) {
        this.openSnackBar("Error!", "Select atleast one quesiton from bank", 3000);
        return;
      }

      this._manageCoursesService.addQuiz(this.addQuizForm.value)
        .subscribe(event => {
          if (event.type === HttpEventType.Sent) {
            this._ngxSpinner.show();
          }
          if (event.type === HttpEventType.Response) {
            this._ngxSpinner.hide();
            this.newQuizClicked = false;
            this.getOutcomeQuizzesFromServer();
            this.openSnackBar("Add Quiz", "Success!", 3000);
          }
        },
          error => {
            this._ngxSpinner.hide();
            this.openSnackBar("Error:", error.error.message, 4000);

          });
    }
    else {
      this.getFormValidationErrors(this.addQuizForm)
    }

  }

  private buildAddQuizForm() {
    this.addQuizForm = this._formBuilder.group({
      Name: ['', [Validators.required]],
      DueDate: ['', [Validators.required]],
      QuestionBankId: ['', [Validators.required]],
      Questions: [''],
      OutcomeId: ['', []],
    })
  }

  get Name() { return this.addQuizForm.get('Name') }
  get DueDate() { return this.addQuizForm.get('DueDate') }
  get Questions() { return this.addQuizForm.get('Questions') }
  get OutcomeId() { return this.addQuizForm.get('OutcomeId') }
  get QuestionBankId() { return this.addQuizForm.get('OutcomeId') }

  private getFormValidationErrors(form: FormGroup) {
    Object.keys(form.controls).forEach(key => {

      const controlErrors: ValidationErrors = form.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
        });
      }
    });
  }

  private openSnackBar(message: string, action: string, _duration: number) {
    this._snackBar.open(message, action, {
      duration: _duration,
      verticalPosition: 'top'
    });
  }

}
