# SLO.App.Mobile

A simple personal shopping list organizer app.

I’ll keep expanding it over time as I explore ideas and shape it into something bigger.

## See it in simple action


https://github.com/user-attachments/assets/5660b2ae-56a5-444d-a68b-5c6717f96cb7



---

### Disclaimer

In this repository/project, I will do my best to make it [![The Standard](https://img.shields.io/github/v/release/hassanhabib/The-Standard?style=default&label=Standard%20Version&color=2ea44f)](https://github.com/hassanhabib/The-Standard/releases/tag=latest) compliant. However, I will still introduce my own thoughts and techniques along the way.

For the official and most accurate documentation, please refer to the original source:  
[The Standard – Latest Release](https://github.com/hassanhabib/The-Standard).

---

# High-Level Design
![SLO MobileApp Components-High Level Design](https://github.com/user-attachments/assets/0c338bac-8e14-4ed2-9655-7beef7da52e4)


## Brokers 
Brokers are the components responsible for interacting with external systems or platform‑level services. They provide a simple, isolated API that the rest of the SLO.MobileApp application can depend on without needing to worry about the underlying provider's implementation.

## Foundation Services
Foundation services are the most inner system components. They sit closest to the outside world and handle the raw interactions that higher‑level services build upon.

## Anotomy of Base Views/Controls
The Base Views/Controls exist to abstract away the underlying UI framework’s implementation.
Each Base View/Control will expose three main characteristics: Properties, Capabilities, and Styles.
This is done in accordance with the Tri‑Nature pattern as demonstrated by The Standard in the Theory Section under the 0.0.4 Fractal Theory.

The Below illustration reflects abstraction, structure, and UI independence.

<img width="1622" height="785" alt="SLO MobileApp Controls" src="https://github.com/user-attachments/assets/af659f97-ccda-4729-87dc-1364fe224e6d" />



## References
- [The Standard](https://github.com/hassanhabib/The-Standard)
- [The Standard Teams](https://github.com/hassanhabib/The-Standard-Team)

## Resources
- [The MVVM Pattern in .NET MAUI: The definitive guide to essential patterns, best practices, and techniques for cross-platform app development](https://www.packtpub.com/en-us/product/the-mvvm-pattern-in-net-maui-9781805125006)
- [.NET MAUI Cookbook](https://www.packtpub.com/en-de/product/net-maui-cookbook-9781835464625?srsltid=AfmBOoo5vnPUx5RG0iZGMLUKkJm7j9eZPUWbCz67NaRKLj1hdn5ZRt6I)
