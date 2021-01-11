module Tello

  open System.Net
  open System.Net.Sockets
  open System.Text
  open System
  open Types

  let outSocket = new UdpClient()
  let mutable remoteAddress = "192.168.10.1"
  let mutable remotePort = 8889

  let GetResponseMsg (socket: UdpClient) = async {
    let! asyncData = socket.ReceiveAsync() |> Async.AwaitTask
    return Some (Data asyncData)
  }

  let SleepFor (milliseconds: int) = async {
    let! _ = System.Threading.Tasks.Task.Delay(milliseconds) |> Async.AwaitTask
    return Some Sleep
  }

  let rec private startMailbox(inbox:MailboxProcessor<RaceWinner>) = begin
    let rec doLoop(outSocket : UdpClient) = async {
            let! input = inbox.Receive()
            match input with
            | Send(command) ->
                let connectMsg = Encoding.ASCII.GetBytes(command)
                let res = outSocket.SendAsync(connectMsg, connectMsg.Length, IPEndPoint(IPAddress.Parse(remoteAddress), remotePort) ) |> Async.AwaitTask
                printfn "CLIENT: Message sent, awaiting response..."
                let! winner = Async.Choice [
                  GetResponseMsg outSocket;
                  SleepFor 10000;
                ]
                match winner with
                | Some Sleep -> 
                  printfn "CLIENT: No response, starting again"
                | Some (Data r) -> 
                  let response = Encoding.ASCII.GetString(r.Buffer)
                  
                  printfn "CLIENT: Response: %s" response
                    // Wait for a second to prevent massive spam
                  do! System.Threading.Tasks.Task.Delay(1000) |> Async.AwaitTask
                | None -> 
                  printfn "CLIENT: Unexpected result, starting again"
                return! doLoop(outSocket)
            | _ ->
                printfn "Invalid Command"
        }

    doLoop(outSocket)
  end

  let private inbox = MailboxProcessor.Start(startMailbox)
 
  let sendCommand (command: string) = 
    printfn ">> %s" command
    inbox.Post(Send(command))

  let takeoff = 
      sendCommand "takeoff"
  
  let init = 
      sendCommand "command"

  let setSpeed speed = 
      sendCommand ("speed " + speed)
   
  let rotatecw degrees = 
      sendCommand ("cw " + degrees)

  let rotateccw degress = 
      sendCommand ("ccw " + degress)

  let flip direction = 
      sendCommand ("flip " + direction)

  let comeDown = 
      sendCommand "land"

  let move direction distance =
      sendCommand (direction + " " + distance)

  let moveBack distance = 
      sendCommand ("back " + distance)

  let moveDown distance = 
      sendCommand ("down " + distance)

  let moveForward distance = 
      sendCommand ("forward" + distance)

  let moveLeft distance = 
      sendCommand ("left " + distance)

  let moveRight distance = 
      sendCommand ("right " + distance)
     
  let moveUp distance = 
      sendCommand ("up " + distance)

  let setIPPort (address : string) port =
    remoteAddress <- address
    remotePort <- port
