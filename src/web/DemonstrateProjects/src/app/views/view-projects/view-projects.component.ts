import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Project } from 'src/app/models/project';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-view-projects',
  templateUrl: './view-projects.component.html',
  styleUrls: ['./view-projects.component.css']
})
export class ViewProjectsComponent implements OnInit {

  constructor(private projectService : ProjectService) { }

  ngOnInit(): void {

  }

  findProjects() {
    const key = (document.getElementById('input-key') as HTMLInputElement).value;
    this.projectService.getByKey(key).subscribe({
      next: (x) => {
        this.projects = x;
      },
      error: () => {
        alert("Error: invalid key or service unavaible.");
      }
    })
  }

  projects? : Project[];
}
