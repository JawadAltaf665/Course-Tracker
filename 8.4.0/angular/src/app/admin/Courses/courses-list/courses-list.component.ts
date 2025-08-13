import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-courses-list',
  templateUrl: './courses-list.component.html',
//  styleUrls: ['./courses-list.component.css']
})
export class CoursesListComponent implements OnInit {
    public coureses = [
    { title: "Python", description: "Python Full Course!" },
    { title: "JavaScript", description: "JavaScript Full Course!" },
    { title: "Java", description: "Java Full Course!" },
    { title: "C++", description: "C++ Full Course!" },
    { title: "C#", description: "C# Full Course!" },
    { title: "PHP", description: "PHP Full Course!" },
    { title: "Ruby", description: "Ruby Full Course!" },
    { title: "Swift", description: "Swift Full Course!" },
    { title: "Go", description: "Go Full Course!" },
    { title: "Kotlin", description: "Kotlin Full Course!" }
]
    constructor() { }

    ngOnInit(): void {
        
    }
    
}
