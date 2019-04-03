declare enum RecordingState {
    "inactive",
    "recording",
    "paused"
}

interface IBlobEvent extends Event {
    data: Blob;
}

interface IMediaRecorderOptions {
    mimeType: string;
    audioBitsPerSecond?: number;
    videoBitsPerSecond?: number;
    bitsPerSecond?: number;
}

interface IMediaRecorder {
    mimeType: string;
    state: string;
    stream: MediaStream;

    onstart: (event: Event) => void;
    onstop: (event: Event) => void;
    ondataavailable: (event: IBlobEvent) => void;
    onpause: (event: Event) => void;
    onresume: (event: Event) => void;
    onerror: (error: Error) => void;

    ignoreMutedMedia: boolean;
    videoBitsPerSecond: number;
    audioBitsPerSecond: number;

    start(timeslice?: number): void;
    stop(): void;
    pause(): void;
    resume(): void;
    requestData(): void;

    isTypeSupported(type: string): boolean;
}

declare var MediaRecorder: {
    prototype: IMediaRecorder;
    new (stream: MediaStream, options?: IMediaRecorderOptions): IMediaRecorder;
    isTypeSupported(type: string): boolean;
}