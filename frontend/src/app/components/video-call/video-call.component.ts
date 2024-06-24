import { Component, Input, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { StreamService } from 'src/app/services/streaming/stream.service';

@Component({
  selector: 'app-video-call',
  templateUrl: './video-call.component.html',
  styleUrls: ['./video-call.component.css'],
})
export class VideoCallComponent implements OnInit, OnDestroy {
  @ViewChild('localVideo', { static: true })
  localVideo: ElementRef<HTMLVideoElement> | any;
  @ViewChild('remoteVideo', { static: true })
  remoteVideo: ElementRef<HTMLVideoElement> | any;

  @Input() selectedFriend: string | null = null;
  private peerConnection!: RTCPeerConnection;
  private localStream!: MediaStream;
  private remoteStream!: MediaStream;

  constructor(private streamService: StreamService) {}

  ngOnInit(): void {
    this.streamService.startConnection();
    this.setupWebRTC();
    this.startLocalStream();

    this.streamService.userList$.subscribe((users) => {
      console.log(users);

      // if (users.length === 0 && this.streamService.connectionId) {
      //   // Register the user if the user list is empty and a connection ID is available
      //   this.streamService.registerUser(this.streamService.connectionId + "- omar");
      //   console.log(users);

      // }
    });


    this.streamService.onReceiveOffer(async (connectionId, offer) => {
      console.log(connectionId);
      console.log('Received offer:', offer);

      try {
        const offerObject =
          typeof offer === 'string' ? JSON.parse(offer) : offer;

        console.log('Parsed offer:', offerObject);

        // Construct RTCSessionDescription
        const sessionDescription = new RTCSessionDescription(offerObject);

        console.log('RTCSessionDescription:', sessionDescription);

        // Set remote description with the offer
        await this.peerConnection.setRemoteDescription(sessionDescription);

        // Create answer
        const answer = await this.peerConnection.createAnswer();

        console.log('Created answer:', answer);

        // Set local description with the answer
        await this.peerConnection.setLocalDescription(answer);

        // Send the answer back
        this.streamService.sendAnswer(connectionId, answer);
      } catch (error) {
        console.error('Error processing offer:', error);
      }
    });

    this.streamService.onReceiveAnswer(async (connectionId, answer) => {
      console.log(answer);
      try {
        const answerObject =
          typeof answer === 'string' ? JSON.parse(answer) : answer;
        console.log('Parsed offer:', answerObject);
        const sessionDescription = new RTCSessionDescription(answerObject);

        console.log('RTCSessionDescription:', sessionDescription);
        await this.peerConnection.setRemoteDescription(sessionDescription);
      } catch (error) {
        console.error('Error processing offer:', error);
      }
    });

    this.streamService.onReceiveIceCandidate((connectionId,candidate) => {
      console.log(connectionId);
      
      this.peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
    });
  }

  async startLocalStream() {
    this.localStream = await navigator.mediaDevices.getUserMedia({
      video: true,
      audio: false,
    });
    this.localVideo.nativeElement.srcObject = this.localStream;

    this.localStream.getTracks().forEach((track: any) => {
      this.peerConnection.addTrack(track, this.localStream);
    });
  }

  setupWebRTC() {
    this.peerConnection = new RTCPeerConnection({
      iceServers: [{ urls: 'stun:stun.l.google.com:19302' }],
    });

    this.peerConnection.onicecandidate = (event: any) => {
      if (event.candidate) {
        if (this.selectedFriend) {
          this.streamService.sendIceCandidate(
            this.selectedFriend, // Send ICE candidate to the selected friend's connection ID
            event.candidate.toJSON()
          );
        }
      }
    };

    this.peerConnection.ontrack = (event: { track: any }) => {
      if (!this.remoteStream) {
        this.remoteStream = new MediaStream();
        this.remoteVideo.nativeElement.srcObject = this.remoteStream;
      }
      this.remoteStream.addTrack(event.track);
    };
  }

  async call() {
    if (!this.selectedFriend) {
      console.error('No friend selected for the call.');
      return;
    }
    console.log(this.selectedFriend);
    const offer = await this.peerConnection.createOffer();
    console.log(offer);

    await this.peerConnection.setLocalDescription(offer);
    this.streamService.sendOffer(this.selectedFriend, offer);
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
      // Stop all tracks in the local stream
      if (this.localStream) {
        this.localStream.getTracks().forEach(track => track.stop());
      }
  
      // Close the peer connection
      if (this.peerConnection) {
        this.peerConnection.close();
      }
  
      console.log('VideoCallComponent destroyed');
    
  }
}
