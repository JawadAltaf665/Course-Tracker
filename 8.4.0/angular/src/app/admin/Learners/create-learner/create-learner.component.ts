import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-learner',
  templateUrl: './create-learner.component.html',
//  styleUrls: ['./create-learner.component.css']
})
export class CreateLearnerComponent implements OnInit {

    public learnerForm: FormGroup;
    public apiUrl = 'https://localhost:44311/api/services/app/Learner';
    public learners: any[] = [];

    public onSave: EventEmitter<void> = new EventEmitter<void>();

    constructor(
        private http: HttpClient,
        private fb: FormBuilder,
        public bsModalRef: BsModalRef
    ) { }

    ngOnInit(): void {
        this.learnerForm = this.fb.group({
            id: 0,
            name: ['', [Validators.required, Validators.minLength(3)]],
            email: ['', [Validators.required, Validators.email]],
        })
    }

    get f() {
        return this.learnerForm.controls;
    }

    Reset() {
        this.learnerForm.reset();
    }

    saveLearner() {
        debugger;
        if (this.learnerForm.invalid) {
            return;
        }

        var formData = this.learnerForm.value;
        console.log(formData);

        if (formData.id == 0) {
            debugger;
            this.http.post(`${this.apiUrl}/CreateLearner`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Learner created successfully!', response);
                        abp.notify.success('Learner created successfully!');
                        this.onSave.emit();

                        this.bsModalRef.hide();
                    },
                    error: (error: any) => {
                        console.error('There was an error creating the learner!', error);
                        abp.notify.error('There was an error creating the learner!');
                    }
                })
        } else {
            debugger;
            this.http.put(`${this.apiUrl}/UpdateLearner`, formData)
                .subscribe({
                    next: (response: any) => {
                        console.log('Learner updated successfully!', response);
                        this.onSave.emit();

                        this.bsModalRef.hide();                    },
                    error: (error: any) => {
                        console.warn('There was an error updating the learner!', error);
                    }
                })
        }

    }

}
