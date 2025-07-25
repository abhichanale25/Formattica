import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormatterService } from '../../services/formatter.service';

@Component({
  selector: 'app-formatter-tool',
  templateUrl: './formatter-tool.component.html',
  styleUrls: ['./formatter-tool.component.css'],
})
export class FormatterToolComponent {
 formatType: string = 'JSON';
  rawInput: string = '';
  formattedOutput: string = '';
  errorMessage: string = '';
  copied: boolean = false;

  constructor(private http: HttpClient,
    private formatterService:FormatterService
  ) {}

  ngOnInit(): void {

  }

  formatContent() {
    this.formattedOutput = '';
    this.errorMessage = '';

    // const payload = {
    //   content: this.rawInput,
    //   formatType: this.formatType,
    // };

    this.formatterService.formatContent(this.rawInput,this.formatType).subscribe({
      next: (res: any) => {
        try {
          const parsed = JSON.parse(res.formatted);
          this.formattedOutput = JSON.stringify(parsed, null, 2); 
        } catch (e) {
          this.formattedOutput = res.formatted; 
        }
      },
      error: (err: any) => {
        this.errorMessage =
          err?.error?.message || 'An error occurred while formatting.';
      }
    });
  }

copyFormattedOutput() {
  if (this.formattedOutput) {
    navigator.clipboard.writeText(this.formattedOutput).then(() => {
      this.copied = true;
      setTimeout(() => this.copied = false, 2000); // hide after 2s
    });
  }
}

autoResize(event: Event): void {
  const textarea = event.target as HTMLTextAreaElement;
  textarea.style.height = 'auto';
  textarea.style.height = textarea.scrollHeight + 'px';
}
}
