module Types


open System.Net
open System.Net.Sockets
open System.Text
open System

// type Tello = {IPAddress: string; Port: int; Socket: UdpClient}

type RaceWinner =
  | Data of UdpReceiveResult
  | Sleep
  | Send of String