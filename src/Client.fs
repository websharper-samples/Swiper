namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.Swiper
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client

[<JavaScript>]
module HelloWorld =

    [<SPAEntryPoint>]
    let Main() =
        let currentSlideIndex = Var.Create 0

        let config = 
            SwipeParameters(
                OnSlideChangeStart = (fun swiper ->
                    Var.Set currentSlideIndex swiper.ActiveIndex
                )
            )
            
        let swiper =
            new WebSharper.Swiper.Swiper(
                ".swiper-container",
                config
            )

        let answerButtons =
            Doc.Concat [
                text "Pick an answer: "
                Doc.Button "A" [] (fun () -> ())
                Doc.Button "B" [] (fun () -> ())
                Doc.Button "C" [] (fun () ->
                    swiper.UnlockSwipeToNext()
                    swiper.SlideNext()
                    let newSlide () =
                        Elt.div [attr.``class`` "swiper-slide"] [
                            text "You have finished the tutorial!"
                        ]
                    swiper.AppendSlide( (newSlide()).Dom )
                )
            ]

        V(match currentSlideIndex.V with
            | 8 ->
                swiper.LockSwipeToNext()
                answerButtons
            | _ ->
                Doc.Empty)
        |> Doc.EmbedView
        |> Doc.RunAppend JS.Document.Body

