# Boundless - Real-time code generation for flexible VR interactions
![Demo](https://github.com/mvnowak/boundless/blob/main/Assets/demo_2.gif)

This project demonstrates the viability of real-time code generation and dynamic execution to allow for a near limitless level of interactivity.
Users can issue voice commands and gestures to specify actions, which are transformed into prompts and sent to an LLM.
Rather than letting the LLM call pre-defined actions, the model is prompted to produce code, which is compiled and executed in real-time using the C# reflection library.

`Whisper.cpp` is run locally for efficient text-to-speech processing. GPT-4o is called via the OpenAI API for code generation.

## Setup
The project was tested using a Meta Quest 2, but since it uses OpenXR it should be compatible with any VR headset.

To use this project, an OpenAI API key has to be inserted via the `Private.cs` class.
